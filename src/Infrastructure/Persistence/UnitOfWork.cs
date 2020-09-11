namespace Aviant.DDD.Infrastructure.Persistence
{
    using System;
    using System.Threading.Tasks;
    using Application.Persistance;
    using Domain.Aggregates;
    using Domain.Services;

    public class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext>, IDisposable
        where TDbContext : IDbContextWrite
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

        #region IUnitOfWork<TDbContext> Members

        public Task<int> Commit()
        {
            return _context.SaveChangesAsync();
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing) _context.Dispose();

            _isDisposed = true;
        }
    }

    public class UnitOfWork<TAggregate, TAggregateId>
        : IUnitOfWork<TAggregate, TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        private readonly IEventsService<TAggregate, TAggregateId> _eventsService;

        public UnitOfWork(IEventsService<TAggregate, TAggregateId> eventsService) =>
            _eventsService = eventsService;

        #region IUnitOfWork<TAggregate,TAggregateId> Members

        public async Task<bool> Commit(TAggregate aggregate)
        {
            await _eventsService.PersistAsync(aggregate)
               .ConfigureAwait(false);

            return true;
        }

        #endregion
    }
}