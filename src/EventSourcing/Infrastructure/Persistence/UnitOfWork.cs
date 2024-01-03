using Aviant.Application.EventSourcing.Persistence;
using Aviant.Core.EventSourcing.Aggregates;
using Aviant.Core.EventSourcing.Services;

namespace Aviant.Infrastructure.EventSourcing.Persistence;

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
