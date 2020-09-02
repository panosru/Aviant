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

        public UnitOfWork(TDbContext context) => _context = context;

    #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    #endregion

    #region IUnitOfWork Members

        public async Task<bool> Commit<TAggregateRoot, TAggregateId>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRoot<TAggregateId>
            where TAggregateId : class, IAggregateId
        {
            try
            {
                if (ServiceLocator.ServiceContainer is null)
                    throw new Exception("ServiceContainer is null");

                var eventsService = ServiceLocator.ServiceContainer
                                                  .GetService<IEventsService<TAggregateRoot, TAggregateId>>(
                                                       typeof(IEventsService<TAggregateRoot, TAggregateId>));

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

    #endregion

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing) _context.Dispose();

            _isDisposed = true;
        }
    }
}