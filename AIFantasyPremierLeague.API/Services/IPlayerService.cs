using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.Services;
public interface IPlayerService
{
    Task<IEnumerable<Player>> GetPlayers();
}