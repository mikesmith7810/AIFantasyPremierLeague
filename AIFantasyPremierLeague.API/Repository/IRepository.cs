namespace AIFantasyPremierLeague.API.Repository;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(string id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> AddAsync(TEntity entity);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}