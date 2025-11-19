using AIFantasyPremierLeague.API.Models;
using AIFantasyPremierLeague.API.Repository;
using AIFantasyPremierLeague.API.Repository.Data;
using AIFantasyPremierLeague.API.Exceptions;

namespace AIFantasyPremierLeague.API.Services;

public class PlayerService(IRepository<PlayerEntity> playerRepository) : IPlayerService
{
    public async Task<Player> AddPlayerAsync(Player player)
    {
        PlayerEntity playerEntity = new() { Id = player.Id, FirstName = player.FirstName, SecondName = player.SecondName, Team = player.TeamId, Price = player.Price, Position = player.Position, PredictedPoints = player.PredictedPoints };

        PlayerEntity response = await playerRepository.AddAsync(playerEntity);

        return new Player(response.Id, response.FirstName, response.SecondName, response.Team, response.Price, response.Position, response.PredictedPoints);

    }

    public async Task<Player> GetPlayerAsync(int playerId)
    {
        PlayerEntity? playerEntity = await playerRepository.GetByIdAsync(playerId) ?? throw new PlayerNotFoundException(playerId.ToString());

        return new Player(playerEntity.Id, playerEntity.FirstName, playerEntity.SecondName, playerEntity.Team, playerEntity.Price, playerEntity.Position, playerEntity.PredictedPoints);
    }

    public async Task<IEnumerable<Player>> GetPlayersAsync()
    {
        var players = await playerRepository.GetAllAsync();

        return players.Select(playerEntity => new Player(playerEntity.Id, playerEntity.FirstName, playerEntity.SecondName, playerEntity.Team, playerEntity.Price, playerEntity.Position, playerEntity.PredictedPoints));
    }
}

