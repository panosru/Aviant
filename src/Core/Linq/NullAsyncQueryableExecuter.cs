namespace Aviant.DDD.Core.Linq
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class NullAsyncQueryableExecutor : IAsyncQueryableExecutor
    {
        public static NullAsyncQueryableExecutor Instance { get; } = new();

        public Task<int> CountAsync<T>(IQueryable<T> queryable) => Task.FromResult(queryable.Count());

        public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable) => Task.FromResult(queryable.ToList());

        public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable) => Task.FromResult(queryable.FirstOrDefault());

        public Task<bool> AnyAsync<T>(IQueryable<T> queryable) => Task.FromResult(queryable.Any());
    }
}