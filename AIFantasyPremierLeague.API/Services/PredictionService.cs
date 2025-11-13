using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.Services;

public class PredictionService : IPredictionService
{
    private readonly IRepository<PlayerEntity> _playerRepository;
    private readonly IRepository<PlayerPerformanceEntity> _playerPerformanceRepository;

    public PredictionService(IRepository<PlayerEntity> playerRepository, IRepository<PlayerPerformanceEntity> playerPerformanceRepository)
    {
        _playerRepository = playerRepository;
        _playerPerformanceRepository = playerPerformanceRepository;
    }

    public async Task<PredictionHighestPoints> GetPredictionHighestPointsAsync()
    {

        var players = new List<Player>
        {
            new Player("player1", "Kane", "team1", 20, "ATT", 29)
        };

        PredictionHighestPoints predictionHighestPoints = new PredictionHighestPoints(players);
        return await Task.FromResult(predictionHighestPoints);
    }
}

