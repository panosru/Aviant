namespace Aviant.DDD.Core.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IAsyncQueryableExecutor
    {
        Task<int> CountAsync<T>(IQueryable<T> queryable);

        Task<List<T>> ToListAsync<T>(IQueryable<T> queryable);

        Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable);

        Task<bool> AnyAsync<T>(IQueryable<T> queryable);
    }
}