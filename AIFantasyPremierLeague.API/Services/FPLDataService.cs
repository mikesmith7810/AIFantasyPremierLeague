using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Exceptions;
using System.Text.Json;

namespace AIFantasyPremierLeague.API.Services;
public class FPLDataService : IFPLDataService
{
    private readonly IRepository<PlayerEntity> _playerRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<FPLDataService> _logger;

    public FPLDataService(IRepository<PlayerEntity> playerRepository, IHttpClientFactory httpClientFactory, ILogger<FPLDataService> logger)
    {
        _playerRepository = playerRepository;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task GetPlayersKnownDataAsync()
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
        _logger.LogInformation("FPL Player Size: {Count}", players.Count);

        foreach (var player in players)
        {
            await _playerRepository.AddAsync(player);
        }

        _logger.LogInformation("Successfully saved {Count} players to database", players.Count);
    }
}

