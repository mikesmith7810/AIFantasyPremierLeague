using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Exceptions;

namespace AIFantasyPremierLeague.API.Services;

public class PlayerPerformanceService(IRepository<PlayerPerformanceEntity> playerPerformanceRepository) : IPlayerPerformanceService
{
    public async Task<PlayerPerformance> AddPlayerPerformanceAsync(PlayerPerformance playerPerformance)
    {

        PlayerPerformanceEntity playerPerformanceEntity = new() { Id = playerPerformance.Id, PlayerId = playerPerformance.PlayerId, Stats = new() { Points = playerPerformance.Points, Goals = playerPerformance.Goals, Assists = playerPerformance.Assists, MinsPlayed = playerPerformance.MinsPlayed } };

        PlayerPerformanceEntity response = await playerPerformanceRepository.AddAsync(playerPerformanceEntity);

        return new PlayerPerformance(response.Id, response.PlayerId, response.Stats.Points, response.Stats.Goals, response.Stats.Assists, response.Stats.MinsPlayed);

    }

    public async Task<PlayerPerformance> GetPlayerPerformanceAsync(string playerPerformanceId)
    {
        PlayerPerformanceEntity? playerPerformanceEntity = await playerPerformanceRepository.GetByIdAsync(playerPerformanceId) ?? throw new PlayerPerformanceNotFoundException(playerPerformanceId);

        return new PlayerPerformance(playerPerformanceEntity.Id, playerPerformanceEntity.PlayerId, playerPerformanceEntity.Stats.Points, playerPerformanceEntity.Stats.Goals, playerPerformanceEntity.Stats.Assists, playerPerformanceEntity.Stats.MinsPlayed);
    }

    public async Task<IEnumerable<PlayerPerformance>> GetPlayerPerformancesAsync()
    {
        IEnumerable<PlayerPerformanceEntity> playerPerformances = await playerPerformanceRepository.GetAllAsync();

        return playerPerformances.Select(playerPerformanceEntity => new PlayerPerformance(playerPerformanceEntity.Id, playerPerformanceEntity.PlayerId, playerPerformanceEntity.Stats.Points, playerPerformanceEntity.Stats.Goals, playerPerformanceEntity.Stats.Assists, playerPerformanceEntity.Stats.MinsPlayed));
    }
}

