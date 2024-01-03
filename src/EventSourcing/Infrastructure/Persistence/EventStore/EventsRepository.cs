using System.Text;
using System.Text.Json;
using Aviant.Core.EventSourcing.Aggregates;
using Aviant.Core.EventSourcing.DomainEvents;
using Aviant.Core.EventSourcing.Persistence;
using Aviant.Core.EventSourcing.Services;
using EventStore.ClientAPI;

namespace Aviant.Infrastructure.EventSourcing.Persistence.EventStore;

internal sealed class EventsRepository<TAggregate, TAggregateId> : IEventsRepository<TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    private readonly IEventStoreConnectionWrapper _connectionWrapper;

    private readonly IEventSerializer _eventSerializer;

    private readonly string _streamBaseName;

    public EventsRepository(IEventStoreConnectionWrapper connectionWrapper, IEventSerializer eventSerializer)
    {
        _connectionWrapper = connectionWrapper;
        _eventSerializer = eventSerializer;

        var aggregateType = typeof(TAggregate);
        _streamBaseName = aggregateType.Name;
    }

    #region IEventsRepository<TAggregate,TAggregateId> Members

    public async Task AppendAsync(
        TAggregate        aggregate,
        CancellationToken cancellationToken = default)
    {
        if (aggregate is null)
            throw new ArgumentNullException(nameof(aggregate));

        if (!aggregate.Events.Any())
            return;

        var streamName = GetStreamName(aggregate.Id);

        IDomainEvent<TAggregateId> firstEvent = aggregate.Events.First();

        var expectedVersion = 0 == firstEvent.AggregateVersion
            ? ExpectedVersion.NoStream
            : firstEvent.AggregateVersion - 1;

        var connection = await _connectionWrapper.GetConnectionAsync(cancellationToken)
           .ConfigureAwait(false);

        using var transaction = await connection.StartTransactionAsync(streamName, expectedVersion)
           .ConfigureAwait(false);

        try
        {
            EventData[] newEvents = aggregate.Events.Select(Map).ToArray();

            await transaction.WriteAsync(newEvents)
               .ConfigureAwait(false);

            await transaction.CommitAsync()
               .ConfigureAwait(false);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<TAggregate?> RehydrateAsync(
        TAggregateId      aggregateId,
        CancellationToken cancellationToken = default)
    {
        var connection = await _connectionWrapper.GetConnectionAsync(cancellationToken)
           .ConfigureAwait(false);

        var streamName = GetStreamName(aggregateId);

        List<IDomainEvent<TAggregateId>> events = new();

        StreamEventsSlice currentSlice;
        long              nextSliceStart = StreamPosition.Start;

        do
        {
            currentSlice = await connection.ReadStreamEventsForwardAsync(
                    streamName,
                    nextSliceStart,
                    200,
                    false)
               .ConfigureAwait(false);

            nextSliceStart = currentSlice.NextEventNumber;

            events.AddRange(currentSlice.Events.Select(Map));
        } while (!currentSlice.IsEndOfStream);

        if (!events.Any())
            return null;

        var result = Aggregate<TAggregate, TAggregateId>.Create(
            events.OrderBy(
                e => e.AggregateVersion));

        return result;
    }

    #endregion

    private string GetStreamName(TAggregateId aggregateId) => $"{_streamBaseName}_{aggregateId}";

    private IDomainEvent<TAggregateId> Map(ResolvedEvent resolvedEvent)
    {
        var meta = JsonSerializer.Deserialize<EventMeta>(resolvedEvent.Event.Metadata);

        return _eventSerializer.Deserialize<TAggregateId>(meta.EventType, resolvedEvent.Event.Data);
    }

    private static EventData Map(IDomainEvent<TAggregateId> @event)
    {
        var json = JsonSerializer.Serialize((dynamic)@event);
        var data = Encoding.UTF8.GetBytes(json);

        var eventType = @event.GetType();

        EventMeta meta = new()
        {
            EventType = eventType.AssemblyQualifiedName!
        };
        var    metaJson = JsonSerializer.Serialize(meta);
        byte[] metadata = Encoding.UTF8.GetBytes(metaJson);

        EventData eventPayload = new(
            Guid.NewGuid(),
            eventType.Name,
            true,
            data,
            metadata);

        return eventPayload;
    }
}

internal struct EventMeta : IEquatable<EventMeta>
{
    public string EventType { get; set; }

    /// <inheritdoc />
    public bool Equals(EventMeta other) => EventType == other.EventType;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is EventMeta other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => EventType.GetHashCode();

    public static bool operator ==(EventMeta left, EventMeta right) => left.Equals(right);

    public static bool operator !=(EventMeta left, EventMeta right) => !left.Equals(right);
}
