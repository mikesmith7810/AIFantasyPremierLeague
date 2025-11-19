using AIFantasyPremierLeague.API.Repository.Config;
using Microsoft.EntityFrameworkCore;
namespace AIFantasyPremierLeague.API.Repository;

public class Repository<TEntity>(AppDbContext context) : IRepository<TEntity> where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public void Update(TEntity entity)
    {
        _dbSet.Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }
}