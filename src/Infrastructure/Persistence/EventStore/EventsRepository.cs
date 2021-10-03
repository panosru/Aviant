namespace Aviant.DDD.Infrastructure.Persistence.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Aggregates;
    using Core.DomainEvents;
    using Core.Persistence;
    using Core.Services;
    using global::EventStore.ClientAPI;

    internal sealed class EventsRepository<TAggregate, TAggregateId> : IEventsRepository<TAggregate, TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        private readonly IEventStoreConnectionWrapper _connectionWrapper;

        private readonly IEventDeserializer _eventDeserializer;

        private readonly string _streamBaseName;

        public EventsRepository(IEventStoreConnectionWrapper connectionWrapper, IEventDeserializer eventDeserializer)
        {
            _connectionWrapper = connectionWrapper;
            _eventDeserializer = eventDeserializer;

            var aggregateType = typeof(TAggregate);
            _streamBaseName = aggregateType.Name;
        }

        #region IEventsRepository<TAggregate,TAggregateId> Members

        public Task AppendAsync(
            TAggregate        aggregate,
            CancellationToken cancellationToken = default)
        {
            if (aggregate is null)
                throw new ArgumentNullException(nameof(aggregate));

            return !aggregate.Events.Any()
                ? Task.CompletedTask
                : AppendAggregateEventsAsync(aggregate, cancellationToken);
        }

        public async Task<TAggregate> RehydrateAsync(
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

            var result = Aggregate<TAggregate, TAggregateId>.Create(
                events.OrderBy(
                    e => e.AggregateVersion));

            return result;
        }

        #endregion

        private async Task AppendAggregateEventsAsync(
            TAggregate        aggregate,
            CancellationToken cancellationToken = default)
        {
            var connection = await _connectionWrapper.GetConnectionAsync(cancellationToken)
               .ConfigureAwait(false);

            var streamName = GetStreamName(aggregate.Id);

            IDomainEvent<TAggregateId> firstEvent = aggregate.Events.First();

            var version = firstEvent.AggregateVersion - 1;

            using var transaction = await connection.StartTransactionAsync(streamName, version)
               .ConfigureAwait(false);

            try
            {
                foreach (var eventData in aggregate.Events.Select(Map))
                    await transaction.WriteAsync(eventData)
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

        private string GetStreamName(TAggregateId aggregateId)
        {
            var streamName = $"{_streamBaseName}_{aggregateId}";

            return streamName;
        }

        private IDomainEvent<TAggregateId> Map(ResolvedEvent resolvedEvent)
        {
            var meta = JsonSerializer.Deserialize<EventMeta>(resolvedEvent.Event.Metadata);

            return _eventDeserializer.Deserialize<TAggregateId>(meta.EventType, resolvedEvent.Event.Data);
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

    internal struct EventMeta
    {
        public string EventType { get; set; }
    }
}
