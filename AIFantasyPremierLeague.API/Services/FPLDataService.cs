using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using System.Text.Json;

namespace AIFantasyPremierLeague.API.Services;

public class FPLDataService(IRepository<PlayerEntity> playerRepository, IRepository<PlayerPerformanceEntity> playerPerformanceRepository, IHttpClientFactory httpClientFactory, ILogger<FPLDataService> logger) : IFPLDataService
{
    private const string FPL_DATA_ENDPOINT = "/api/bootstrap-static/";
    private const string FPL_API = "FPLApi";
    private const string FPL_EVENTS_ENDPOINT = "/api/event/";
    private const string LIVE = "/live/";
    public async Task LoadPlayersKnownDataAsync()
    {
        var response = await httpClientFactory.CreateClient(FPL_API).GetAsync(FPL_DATA_ENDPOINT);

        var jsonString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        FPLData? fplData = JsonSerializer.Deserialize<FPLData>(jsonString, options);

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

    public async Task LoadPlayersPerformanceDataAsync(int gameWeek)
    {
        var response = await httpClientFactory.CreateClient(FPL_API).GetAsync(FPL_EVENTS_ENDPOINT + gameWeek + LIVE);

        var jsonString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        FPLPerformanceData? fplPerformanceData = JsonSerializer.Deserialize<FPLPerformanceData>(jsonString, options);

        if (fplPerformanceData?.PlayerPerformances == null)
        {
            logger.LogWarning("No player performance elements found in FPL API response");
            return;
        }

        var playerPerformanceEntities = fplPerformanceData.PlayerPerformances
            .Select(playerPerformance => new PlayerPerformanceEntity
            {
                Id = playerPerformance.Id,
                PlayerId = playerPerformance.PlayerId,
                Stats = playerPerformance.Stats,
                GameWeek = gameWeek
            })
            .ToList();

        foreach (var playerPerformanceEntity in playerPerformanceEntities)
        {
            await playerPerformanceRepository.AddAsync(playerPerformanceEntity);
        }


    }
}

