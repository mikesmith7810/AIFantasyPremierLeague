using AIFantasyPremierLeague.API.Repository.Config;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace AIFantasyPremierLeague.API.Repository;

public class TeamFixtureRepository(AppDbContext context) : Repository<TeamFixtureEntity>(context), ITeamFixtureRepository
{
    private readonly DbSet<TeamFixtureEntity> _dbSet = context.Set<TeamFixtureEntity>();



    public async Task<TeamFixtureEntity?> GetTeamFixtureByGameWeekAsync(int TeamId, int GameWeek)
    {
        return await _dbSet
            .Where(tf => tf.Week == GameWeek && (tf.TeamHome == TeamId || tf.TeamAway == TeamId))
            .FirstOrDefaultAsync();
    }
}