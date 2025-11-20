using AIFantasyPremierLeague.API.Repository.Data;

namespace AIFantasyPremierLeague.API.Repository;

public interface ITeamFixtureRepository : IRepository<TeamFixtureEntity>
{
    Task<TeamFixtureEntity> GetTeamFixtureByGameWeekAsync(int TeamId, int GameWeek);
}