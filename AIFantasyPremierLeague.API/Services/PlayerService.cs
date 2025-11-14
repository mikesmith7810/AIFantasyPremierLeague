using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Exceptions;

namespace AIFantasyPremierLeague.API.Services;

public class PlayerService(IRepository<PlayerEntity> playerRepository) : IPlayerService
{
    public async Task<Player> AddPlayerAsync(Player player)
    {
        PlayerEntity playerEntity = new() { Id = player.Id, Name = player.Name, Team = player.TeamId, Value = player.Value, Position = player.Position, PredictedPoints = player.PredictedPoints };

        PlayerEntity response = await playerRepository.AddAsync(playerEntity);

        return new Player(response.Id, response.Name, response.Team, response.Value, response.Position, response.PredictedPoints);

    }

    public async Task<Player> GetPlayerAsync(string playerId)
    {
        PlayerEntity? playerEntity = await playerRepository.GetByIdAsync(playerId) ?? throw new PlayerNotFoundException(playerId);

        return new Player(playerEntity.Id, playerEntity.Name, playerEntity.Team, playerEntity.Value, playerEntity.Position, playerEntity.PredictedPoints);
    }

    public async Task<IEnumerable<Player>> GetPlayersAsync()
    {
        var players = await playerRepository.GetAllAsync();

        return players.Select(playerEntity => new Player(playerEntity.Id, playerEntity.Name, playerEntity.Team, playerEntity.Value, playerEntity.Position, playerEntity.PredictedPoints));
    }
}

