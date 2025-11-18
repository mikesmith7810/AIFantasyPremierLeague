using AIFantasyPremierLeague.API.Repository.Config;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace AIFantasyPremierLeague.API.Repository;

public class TeamHistoryRepository(AppDbContext context) : Repository<TeamHistoryEntity>(context), ITeamHistoryRepository
{
    private readonly DbSet<TeamHistoryEntity> _dbSet = context.Set<TeamHistoryEntity>();

    public async Task<IEnumerable<TeamHistoryEntity>> GetLastNWeeksForTeamHistoryAsync(int TeamId, int NumberOfWeeks)
    {
        return await _dbSet
           .Where(th => th.TeamId == TeamId)
           .OrderByDescending(th => th.Week)
           .Take(NumberOfWeeks)
           .ToListAsync();
    }
}