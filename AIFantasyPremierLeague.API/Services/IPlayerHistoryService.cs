using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.Services;
public interface IPlayerHistoryService
{
    Task<IEnumerable<PlayerHistory>> GetPlayerHistorysAsync();

    Task<PlayerHistory> AddPlayerHistoryAsync(PlayerHistory playerHistory);

    Task<PlayerHistory> GetPlayerHistoryAsync(string playerHistoryId);
}