using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.EntityFrameworkCore;
namespace AIFantasyPremierLeague.API.Repository.Config;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<TeamEntity> Teams { get; set; }

    public DbSet<TeamHistoryEntity> TeamHistorys { get; set; }

    public DbSet<PlayerEntity> Players { get; set; }

    public DbSet<PlayerPerformanceEntity> PlayerHistory { get; set; }
}