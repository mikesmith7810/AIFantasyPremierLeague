using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Exceptions;

namespace AIFantasyPremierLeague.API.Services;
public class PlayerHistoryService : IPlayerHistoryService
{
    private readonly IRepository<PlayerHistoryEntity> _playerHistoryRepository;
    public PlayerHistoryService(IRepository<PlayerHistoryEntity> playerHistoryRepository)
    {
        _playerHistoryRepository = playerHistoryRepository;
    }

    public async Task<PlayerHistory> AddPlayerHistoryAsync(PlayerHistory playerHistory)
    {
        PlayerHistoryEntity playerHistoryEntity = new() { Id = playerHistory.Id, PlayerId = playerHistory.PlayerId, Season = playerHistory.Season, Week = playerHistory.Week, Team = playerHistory.TeamId, Points = playerHistory.Points, Goals = playerHistory.Goals, Assists = playerHistory.Assists, MinsPlayed = playerHistory.MinsPlayed };

        PlayerHistoryEntity response = await _playerHistoryRepository.AddAsync(playerHistoryEntity);

        return new PlayerHistory(response.Id, response.PlayerId, response.Season, response.Week, response.Team, response.Points, response.Goals, response.Assists, response.MinsPlayed);

    }

    public async Task<PlayerHistory> GetPlayerHistoryAsync(string playerHistoryId)
    {
        PlayerHistoryEntity? playerHistoryEntity = await _playerHistoryRepository.GetByIdAsync(playerHistoryId);

        if (playerHistoryEntity == null)
            throw new PlayerHistoryNotFoundException(playerHistoryId);

        return new PlayerHistory(playerHistoryEntity.Id, playerHistoryEntity.PlayerId, playerHistoryEntity.Season, playerHistoryEntity.Week, playerHistoryEntity.Team, playerHistoryEntity.Points, playerHistoryEntity.Goals, playerHistoryEntity.Assists, playerHistoryEntity.MinsPlayed);
    }

    public async Task<IEnumerable<PlayerHistory>> GetPlayerHistorysAsync()
    {
        IEnumerable<PlayerHistoryEntity> playerHistorys = await _playerHistoryRepository.GetAllAsync();
        return playerHistorys.Select(playerHistoryEntity => new PlayerHistory(playerHistoryEntity.Id, playerHistoryEntity.PlayerId, playerHistoryEntity.Season, playerHistoryEntity.Week, playerHistoryEntity.Team, playerHistoryEntity.Points, playerHistoryEntity.Goals, playerHistoryEntity.Assists, playerHistoryEntity.MinsPlayed));
    }
}

