using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.Repository;

public interface IPlayerPerformanceRepository : IRepository<PlayerPerformanceEntity>
{
    Task<IEnumerable<PlayerPerformanceEntity>> GetByPlayerIdAsync(string playerId);
    Task<IEnumerable<PlayerPerformanceEntity>> GetLastNWeeksForPlayerAsync(string playerId, int numberOfWeeks);
    Task<int> GetTeamTotalPointsConcededForGameWeek(int teamId, int week);
}