using AIFantasyPremierLeague.API.Models;

namespace AIFantasyPremierLeague.API.Services;
public interface IPlayerPerformanceService
{
    Task<IEnumerable<PlayerPerformance>> GetPlayerPerformancesAsync();

    Task<PlayerPerformance> AddPlayerPerformanceAsync(PlayerPerformance playerPerformance);

    Task<PlayerPerformance> GetPlayerPerformanceAsync(string playerPerformanceId);
}