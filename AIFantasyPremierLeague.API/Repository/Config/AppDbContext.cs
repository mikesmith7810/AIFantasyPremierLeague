// Data/AppDbContext.cs
using AIFantasyPremierLeague.API.Repository.Data;
using Microsoft.EntityFrameworkCore;
namespace AIFantasyPremierLeague.API.Repository.Config;

public class AppDbContext : DbContext
{
    public DbSet<TeamEntity> Teams { get; set; }

    public DbSet<PlayerEntity> Players { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}