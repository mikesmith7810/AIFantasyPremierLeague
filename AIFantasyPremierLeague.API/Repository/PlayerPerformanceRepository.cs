using AIFantasyPremierLeague.API.Repository.Config;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace AIFantasyPremierLeague.API.Repository;

public class PlayerPerformanceRepository(AppDbContext context) : Repository<PlayerPerformanceEntity>(context), IPlayerPerformanceRepository
{
    private readonly DbSet<PlayerPerformanceEntity> _dbSet = context.Set<PlayerPerformanceEntity>();

    public async Task<IEnumerable<PlayerPerformanceEntity>> GetByPlayerIdAsync(string playerId)
    {
        return await _dbSet.Where(p => p.PlayerId == playerId).ToListAsync();
    }

    public async Task<IEnumerable<PlayerPerformanceEntity>> GetLastNWeeksForPlayerAsync(string playerId, int numberOfWeeks)
    {
        return await _dbSet
            .Where(p => p.PlayerId == playerId)
            .OrderByDescending(p => p.GameWeek)
            .Take(numberOfWeeks)
            .ToListAsync();
    }

    public async Task<int> GetTeamTotalPointsConcededForGameWeek(int teamId, int week)
    {
        return await _dbSet
            .Where(p => p.OpponentTeam == teamId && p.GameWeek == week)
            .SumAsync(p => p.Stats.Points);
    }
}