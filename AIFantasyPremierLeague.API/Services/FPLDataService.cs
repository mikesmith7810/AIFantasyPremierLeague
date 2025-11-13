using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Exceptions;
using System.Text.Json;

namespace AIFantasyPremierLeague.API.Services;
public class FPLDataService : IFPLDataService
{
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

        var client = _httpClientFactory.CreateClient("FPLApi");

        var response = await client.GetAsync("/api/bootstrap-static/");

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

        List<PlayerEntity> players = fplData.Players;

        foreach (var player in players)
        {
            await _playerRepository.AddAsync(player);
        }
    }

    public async Task LoadPlayersPerformanceDataAsync(string gameWeek)
    {

        var client = _httpClientFactory.CreateClient("FPLApi");

        var response = await client.GetAsync("/api/event/" + gameWeek + "/live/");

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

        List<PlayerPerformanceEntity> playerPerformances = fplPerformanceData.PlayerPerformances;

        foreach (var playerPerformance in playerPerformances)
        {
            await _playerPerformanceRepository.AddAsync(playerPerformance);
        }


    }
}

