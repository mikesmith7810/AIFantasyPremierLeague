using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.Repository;

public interface ITeamHistoryRepository : IRepository<TeamHistoryEntity>
{
    Task<IEnumerable<TeamHistoryEntity>> GetLastNWeeksForTeamHistoryAsync(int TeamId, int NumberOfWeeks);
    Task<IEnumerable<TeamHistoryEntity>> GetLastNWeeksForTeamHistoryByGameWeekAsync(int TeamId, int NumberOfWeeks, int GameWeek);
}