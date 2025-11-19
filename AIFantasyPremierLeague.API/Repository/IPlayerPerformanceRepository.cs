using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.Repository;

public interface IPlayerPerformanceRepository : IRepository<PlayerPerformanceEntity>
{
    Task<IEnumerable<PlayerPerformanceEntity>> GetByPlayerIdAsync(int PlayerId);
    Task<IEnumerable<PlayerPerformanceEntity>> GetLastNWeeksForPlayerAsync(int PlayerId, int NumberOfWeeks);

    Task<IEnumerable<PlayerPerformanceEntity>> GetLastNWeeksForPlayerByGameWeekAsync(int PlayerId, int NumberOfWeeks, int GameWeek);
    Task<int> GetTeamTotalPointsConcededForGameWeek(int teamId, int week);
}