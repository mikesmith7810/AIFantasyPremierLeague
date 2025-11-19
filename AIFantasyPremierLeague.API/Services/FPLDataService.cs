using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using System.Text.Json;

namespace AIFantasyPremierLeague.API.Services;

public class FPLDataService(IRepository<PlayerEntity> playerRepository, IRepository<TeamEntity> teamRepository, IRepository<TeamHistoryEntity> teamHistoryRepository, IPlayerPerformanceRepository playerPerformanceRepository, IHttpClientFactory httpClientFactory, ILogger<FPLDataService> logger) : IFPLDataService
{
    private const string FPL_DATA_ENDPOINT = "/api/bootstrap-static/";
    private const string FPL_API = "FPLApi";
    private const string FPL_EVENTS_ENDPOINT = "/api/event/";
    private const string FPL_DETAILED_PLAYER_ENDPOINT = "/api/element-summary/";
    private const string LIVE = "/live/";

    public async Task LoadPlayersKnownDataAsync()
    {
        var fplPlayerKnownDataResponse = await httpClientFactory.CreateClient(FPL_API).GetAsync(FPL_DATA_ENDPOINT);

        var fplPlayerKnownDataJson = await fplPlayerKnownDataResponse.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        FPLData? fplData = JsonSerializer.Deserialize<FPLData>(fplPlayerKnownDataJson, options);

        if (fplData?.Players == null)
        {
            logger.LogWarning("No player elements found in FPL API response");
            return;
        }

        foreach (var player in fplData.Players)
        {
            await playerRepository.AddAsync(player);
        }
    }

    public async Task LoadTeamsKnownDataAsync()
    {
        var response = await httpClientFactory.CreateClient(FPL_API).GetAsync(FPL_DATA_ENDPOINT);

        var jsonString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        FPLTeamData? fplData = JsonSerializer.Deserialize<FPLTeamData>(jsonString, options);

        if (fplData?.Teams == null)
        {
            logger.LogWarning("No team elements found in FPL API response");
            return;
        }

        foreach (var team in fplData.Teams)
        {
            await teamRepository.AddAsync(team);
        }
    }

    public async Task LoadTeamFixtureHistoryData(int gameWeek)
    {
        IEnumerable<TeamEntity> teams = await teamRepository.GetAllAsync();

        foreach (var team in teams)
        {
            var teamTotalPointsConceded = await playerPerformanceRepository.GetTeamTotalPointsConcededForGameWeek(team.Id, gameWeek);
            var teamHistoryEntity = new TeamHistoryEntity
            {
                Id = $"teamHistory_{Guid.NewGuid().ToString("N")[..8]}",
                TeamId = team.Id,
                Week = gameWeek,
                PointsConceded = teamTotalPointsConceded
            };
            await teamHistoryRepository.AddAsync(teamHistoryEntity);
        }
    }

    public async Task LoadPlayersPerformanceDataAsync(int gameWeek)
    {
        var fplPlayerData = await httpClientFactory.CreateClient(FPL_API).GetAsync(FPL_EVENTS_ENDPOINT + gameWeek + LIVE);

        var fplPlayerDataJSON = await fplPlayerData.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        FPLPlayerEventResponse? playerDataResponseData = JsonSerializer.Deserialize<FPLPlayerEventResponse>(fplPlayerDataJSON, options);

        if (playerDataResponseData?.Elements == null)
        {
            logger.LogWarning("No player performance elements found in FPL API response");
            return;
        }

        var playerDataList = playerDataResponseData.Elements.Select(element => new FPLPlayerEventData
        {
            PlayerId = element.Id,
            Week = gameWeek,
            FixtureId = element.Explain.FirstOrDefault()?.Fixture ?? 0,
            Minutes = element.Stats.Minutes,
            GoalsScored = element.Stats.GoalsScored,
            Assists = element.Stats.Assists,
            TotalPoints = element.Stats.TotalPoints
        }).ToList();

        var playerPerformanceEntities = new List<PlayerPerformanceEntity>();

        foreach (var playerData in playerDataList)
        {
            var fplPlayerDetailedData = await httpClientFactory.CreateClient(FPL_API).GetAsync(FPL_DETAILED_PLAYER_ENDPOINT + playerData.PlayerId);
            var fplPlayerDetailedDataJSON = await fplPlayerDetailedData.Content.ReadAsStringAsync();

            var options2 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };

            FPLPlayerDetailedDataResponse? playerDetailedDataResponseData = JsonSerializer.Deserialize<FPLPlayerDetailedDataResponse>(fplPlayerDetailedDataJSON, options2);

            // Filter detailed data to only include records for the current gameweek
            var currentGameWeekDetailedData = playerDetailedDataResponseData?.Historys?
                .FirstOrDefault(h => h.Round == gameWeek);

            if (currentGameWeekDetailedData != null)
            {
                var playerPerformanceEntity = new PlayerPerformanceEntity
                {
                    PlayerId = playerData.PlayerId,
                    GameWeek = playerData.Week,
                    FixtureId = playerData.FixtureId,
                    OpponentTeam = currentGameWeekDetailedData.OpponentTeam,
                    IsHome = currentGameWeekDetailedData.WasHome ? 1 : 0,
                    Position = (int)((await playerRepository.GetByIdAsync(playerData.PlayerId))?.Position ?? 0),
                    Stats = new PlayerStats
                    {
                        MinsPlayed = playerData.Minutes,
                        Goals = playerData.GoalsScored,
                        Assists = playerData.Assists,
                        Points = playerData.TotalPoints,
                        Bonus = currentGameWeekDetailedData.Bonus,
                        CleanSheets = currentGameWeekDetailedData.CleanSheets,
                        GoalsConceded = currentGameWeekDetailedData.GoalsConceded,
                        YellowCards = currentGameWeekDetailedData.YellowCards,
                        RedCards = currentGameWeekDetailedData.RedCards,
                        Saves = currentGameWeekDetailedData.Saves
                    }
                };

                playerPerformanceEntities.Add(playerPerformanceEntity);
            }
        }

        foreach (var playerPerformanceEntity in playerPerformanceEntities)
        {
            await playerPerformanceRepository.AddAsync(playerPerformanceEntity);
        }
    }
}

