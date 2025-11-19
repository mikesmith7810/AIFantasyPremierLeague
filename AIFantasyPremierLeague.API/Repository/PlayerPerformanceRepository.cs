using AIFantasyPremierLeague.API.Repository.Config;
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace AIFantasyPremierLeague.API.Repository;

public class PlayerPerformanceRepository(AppDbContext context) : Repository<PlayerPerformanceEntity>(context), IPlayerPerformanceRepository
{
    private readonly DbSet<PlayerPerformanceEntity> _dbSet = context.Set<PlayerPerformanceEntity>();

    public async Task<IEnumerable<PlayerPerformanceEntity>> GetByPlayerIdAsync(int PlayerId)
    {
        return await _dbSet.Where(p => p.PlayerId == PlayerId).ToListAsync();
    }

    public async Task<IEnumerable<PlayerPerformanceEntity>> GetLastNWeeksForPlayerAsync(int PlayerId, int NumberOfWeeks)
    {
        return await _dbSet
            .Where(p => p.PlayerId == PlayerId)
            .OrderByDescending(p => p.GameWeek)
            .Take(NumberOfWeeks)
            .ToListAsync();
    }

    public async Task<IEnumerable<PlayerPerformanceEntity>> GetLastNWeeksForPlayerByGameWeekAsync(int PlayerId, int NumberOfWeeks, int GameWeek)
    {
        return await _dbSet
            .Where(p => p.PlayerId == PlayerId && p.GameWeek < GameWeek)
            .OrderByDescending(p => p.GameWeek)
            .Take(NumberOfWeeks)
            .ToListAsync();
    }

    public async Task<int> GetTeamTotalPointsConcededForGameWeek(int teamId, int week)
    {
        return await _dbSet
            .Where(p => p.OpponentTeam == teamId && p.GameWeek == week)
            .SumAsync(p => p.Stats.Points);
    }
}