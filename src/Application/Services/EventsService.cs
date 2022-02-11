namespace Aviant.Application.Services;

using Core.Aggregates;
using Core.EventBus;
using Core.Persistence;
using Core.Services;

public sealed class EventsService<TAggregate, TAggregateId> : IEventsService<TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    private readonly IEventProducer<TAggregate, TAggregateId> _eventProducer;

    private readonly IEventsRepository<TAggregate, TAggregateId> _eventsRepository;

    public EventsService(
        IEventsRepository<TAggregate, TAggregateId> eventsRepository,
        IEventProducer<TAggregate, TAggregateId>    eventProducer)
    {
        _eventsRepository = eventsRepository ?? throw new ArgumentNullException(nameof(eventsRepository));

        _eventProducer = eventProducer ?? throw new ArgumentNullException(nameof(eventProducer));
    }

    #region IEventsService<TAggregate,TAggregateId> Members

    public Task PersistAsync(
        TAggregate        aggregate,
        CancellationToken cancellationToken = default)
    {
        if (aggregate is null)
            throw new ArgumentNullException(nameof(aggregate));

        return !aggregate.Events.Any()
            ? Task.CompletedTask
            : PersistEventsAsync(aggregate, cancellationToken);
    }

    public Task<TAggregate?> RehydrateAsync(
        TAggregateId      key,
        CancellationToken cancellationToken = default) =>
        _eventsRepository.RehydrateAsync(key, cancellationToken);

    #endregion

    private async Task PersistEventsAsync(
        TAggregate        aggregate,
        CancellationToken cancellationToken = default)
    {
        await _eventsRepository.AppendAsync(aggregate, cancellationToken)
           .ConfigureAwait(false);

        await _eventProducer.DispatchAsync(aggregate, cancellationToken)
           .ConfigureAwait(false);
    }
}
