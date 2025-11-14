using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Exceptions;
using System.Text.Json;

namespace AIFantasyPremierLeague.API.Services;
public class FPLDataService : IFPLDataService
{
    private const string FPL_DATA_ENDPOINT = "/api/bootstrap-static/";
    private const string FPL_API = "FPLApi";
    private const string FPL_EVENTS_ENDPOINT = "/api/event/";
    private const string LIVE = "/live/";
    private readonly IRepository<PlayerEntity> _playerRepository;

    private readonly IRepository<PlayerPerformanceEntity> _playerPerformanceRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<FPLDataService> _logger;

    public FPLDataService(IRepository<PlayerEntity> playerRepository, IRepository<PlayerPerformanceEntity> playerPerformanceRepository, IHttpClientFactory httpClientFactory, ILogger<FPLDataService> logger)
    {
        _playerRepository = playerRepository;
        _playerPerformanceRepository = playerPerformanceRepository;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task LoadPlayersKnownDataAsync()
    {
        var response = await _httpClientFactory.CreateClient(FPL_API).GetAsync(FPL_DATA_ENDPOINT);

        var jsonString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        FPLData? fplData = JsonSerializer.Deserialize<FPLData>(jsonString, options);

        if (fplData?.Players == null)
        {
            _logger.LogWarning("No player elements found in FPL API response");
            return;
        }

        foreach (var player in fplData.Players)
        {
            await _playerRepository.AddAsync(player);
        }
    }

    public async Task LoadPlayersPerformanceDataAsync(int gameWeek)
    {
        var response = await _httpClientFactory.CreateClient(FPL_API).GetAsync(FPL_EVENTS_ENDPOINT + gameWeek + LIVE);

        var jsonString = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true
        };

        FPLPerformanceData? fplPerformanceData = JsonSerializer.Deserialize<FPLPerformanceData>(jsonString, options);

        if (fplPerformanceData?.PlayerPerformances == null)
        {
            _logger.LogWarning("No player performance elements found in FPL API response");
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
            await _playerPerformanceRepository.AddAsync(playerPerformanceEntity);
        }


    }
}

