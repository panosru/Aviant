namespace Aviant.Core.Linq;

public interface IAsyncQueryableExecutor
{
    Task<int> CountAsync<T>(IQueryable<T> queryable);

    Task<List<T>> ToListAsync<T>(IQueryable<T> queryable);

    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable);

    Task<bool> AnyAsync<T>(IQueryable<T> queryable);
}
