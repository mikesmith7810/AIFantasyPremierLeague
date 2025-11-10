using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.Services;
public class PlayerService : IPlayerService
{
    private readonly IRepository<PlayerEntity> _playerRepository;
    public PlayerService(IRepository<PlayerEntity> playerRepository)
    {
        _playerRepository = playerRepository;
    }
    public async Task<IEnumerable<Player>> GetPlayers()
    {
        IEnumerable<PlayerEntity> players = await _playerRepository.GetAllAsync();
        return players.Select(playerEntity => new Player(playerEntity.Id, playerEntity.Name, playerEntity.Team));
    }
}

