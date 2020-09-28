namespace Aviant.DDD.Infrastructure.Persistence
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Persistance;
    using Core.Aggregates;
    using Core.Services;

    public sealed class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext>, IDisposable
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

        ~UnitOfWork() => Dispose(false);

        #endregion

        #region IUnitOfWork<TDbContext> Members

        public async Task<int> CommitAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken)
               .ConfigureAwait(false);

        #endregion

        private void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing) _context.Dispose();

            _isDisposed = true;
        }
    }

    public sealed class UnitOfWork<TAggregate, TAggregateId>
        : IUnitOfWork<TAggregate, TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        private readonly IEventsService<TAggregate, TAggregateId> _eventsService;

        public UnitOfWork(IEventsService<TAggregate, TAggregateId> eventsService) =>
            _eventsService = eventsService;

        #region IUnitOfWork<TAggregate,TAggregateId> Members

        public async Task<bool> CommitAsync(
            TAggregate        aggregate,
            CancellationToken cancellationToken = default)
        {
            await _eventsService.PersistAsync(aggregate, cancellationToken)
               .ConfigureAwait(false);

            return true;
        }

        #endregion
    }
}