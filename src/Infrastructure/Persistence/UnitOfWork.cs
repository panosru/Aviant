namespace Aviant.DDD.Infrastructure.Persistence
{
    using System;
    using System.Threading.Tasks;
    using Application.Persistance;
    using Domain.Aggregates;
    using Domain.Persistence;
    using Domain.Services;

    public class UnitOfWork<TDbContext> : IUnitOfWork, IDisposable
        where TDbContext : IApplicationDbContext
    {
        private readonly TDbContext _context;
        private bool _isDisposed;

        public UnitOfWork(TDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        public async Task<bool> Commit<TAggregateRoot, TKey>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRoot<TKey>
        {
            try
            {
                if (ServiceLocator.ServiceContainer is null)
                    throw new Exception("ServiceContainer is null");
            
                var eventsService = ServiceLocator.ServiceContainer.GetService<IEventsService<TAggregateRoot, TKey>>(
                    typeof(IEventsService<TAggregateRoot, TKey>));

                await eventsService.PersistAsync(aggregateRoot);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> Commit()
        {
            try
            {
                var affectedRows = await _context.SaveChangesAsync()
                    .ConfigureAwait(false);

                return affectedRows;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing) _context.Dispose();

            _isDisposed = true;
        }
    }
}