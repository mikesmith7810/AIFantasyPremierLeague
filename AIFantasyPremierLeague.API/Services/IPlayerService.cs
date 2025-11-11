using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.Services;
public interface IPlayerService
{
    Task<IEnumerable<Player>> GetPlayersAsync();

    Task<Player> AddPlayerAsync(Player player);

    Task<Player> GetPlayerAsync(string playerId);
}