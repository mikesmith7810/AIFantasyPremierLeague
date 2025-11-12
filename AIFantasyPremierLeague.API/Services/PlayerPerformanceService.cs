using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Exceptions;

namespace AIFantasyPremierLeague.API.Services;

public class PlayerPerformanceService : IPlayerPerformanceService
{
    private readonly IRepository<PlayerPerformanceEntity> _playerPerformanceRepository;
    public PlayerPerformanceService(IRepository<PlayerPerformanceEntity> playerPerformanceRepository)
    {
        _playerPerformanceRepository = playerPerformanceRepository;
    }

    public async Task<PlayerPerformance> AddPlayerPerformanceAsync(PlayerPerformance playerPerformance)
    {
        PlayerPerformanceEntity playerPerformanceEntity = new() { Id = playerPerformance.Id, PlayerId = playerPerformance.PlayerId, Season = playerPerformance.Season, Week = playerPerformance.Week, Team = playerPerformance.TeamId, Points = playerPerformance.Points, Goals = playerPerformance.Goals, Assists = playerPerformance.Assists, MinsPlayed = playerPerformance.MinsPlayed };

        PlayerPerformanceEntity response = await _playerPerformanceRepository.AddAsync(playerPerformanceEntity);

        return new PlayerPerformance(response.Id, response.PlayerId, response.Season, response.Week, response.Team, response.Points, response.Goals, response.Assists, response.MinsPlayed);

    }

    public async Task<PlayerPerformance> GetPlayerPerformanceAsync(string playerPerformanceId)
    {
        PlayerPerformanceEntity? playerPerformanceEntity = await _playerPerformanceRepository.GetByIdAsync(playerPerformanceId);

        if (playerPerformanceEntity == null)
            throw new PlayerPerformanceNotFoundException(playerPerformanceId);

        return new PlayerPerformance(playerPerformanceEntity.Id, playerPerformanceEntity.PlayerId, playerPerformanceEntity.Season, playerPerformanceEntity.Week, playerPerformanceEntity.Team, playerPerformanceEntity.Points, playerPerformanceEntity.Goals, playerPerformanceEntity.Assists, playerPerformanceEntity.MinsPlayed);
    }

    public async Task<IEnumerable<PlayerPerformance>> GetPlayerPerformancesAsync()
    {
        IEnumerable<PlayerPerformanceEntity> playerPerformances = await _playerPerformanceRepository.GetAllAsync();
        return playerPerformances.Select(playerPerformanceEntity => new PlayerPerformance(playerPerformanceEntity.Id, playerPerformanceEntity.PlayerId, playerPerformanceEntity.Season, playerPerformanceEntity.Week, playerPerformanceEntity.Team, playerPerformanceEntity.Points, playerPerformanceEntity.Goals, playerPerformanceEntity.Assists, playerPerformanceEntity.MinsPlayed));
    }
}

