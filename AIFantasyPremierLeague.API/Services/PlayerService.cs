using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Exceptions;

namespace AIFantasyPremierLeague.API.Services;
public class PlayerService : IPlayerService
{
    private readonly IRepository<PlayerEntity> _playerRepository;
    public PlayerService(IRepository<PlayerEntity> playerRepository)
    {
        _playerRepository = playerRepository;
    }

    public async Task<Player> AddPlayerAsync(Player player)
    {
        PlayerEntity playerEntity = new() { Id = player.Id, Name = player.Name, Team = player.TeamId, Value = player.Value };

        PlayerEntity response = await _playerRepository.AddAsync(playerEntity);

        return new Player(response.Id, response.Name, response.Team, response.Value);

    }

    public async Task<Player> GetPlayerAsync(string playerId)
    {
        PlayerEntity? playerEntity = await _playerRepository.GetByIdAsync(playerId);

        if (playerEntity == null)
            throw new PlayerNotFoundException(playerId);

        return new Player(playerEntity.Id, playerEntity.Name, playerEntity.Team, playerEntity.Value);
    }

    public async Task<IEnumerable<Player>> GetPlayersAsync()
    {
        IEnumerable<PlayerEntity> players = await _playerRepository.GetAllAsync();
        return players.Select(playerEntity => new Player(playerEntity.Id, playerEntity.Name, playerEntity.Team, playerEntity.Value));
    }
}

