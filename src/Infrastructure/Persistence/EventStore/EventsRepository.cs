namespace Aviant.DDD.Infrastructure.Persistence.EventStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Domain.Aggregates;
    using Domain.Events;
    using Domain.Persistence;
    using Domain.Services;
    using global::EventStore.ClientAPI;

    public class EventsRepository<TAggregateRoot, TKey> : IEventsRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        private readonly IEventStoreConnectionWrapper _connectionWrapper;
        private readonly IEventDeserializer _eventDeserializer;
        private readonly string _streamBaseName;

        public EventsRepository(IEventStoreConnectionWrapper connectionWrapper, IEventDeserializer eventDeserializer)
        {
            _connectionWrapper = connectionWrapper;
            _eventDeserializer = eventDeserializer;

            var aggregateType = typeof(TAggregateRoot);
            _streamBaseName = aggregateType.Name;
        }

        public async Task AppendAsync(TAggregateRoot aggregateRoot)
        {
            if (aggregateRoot is null)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            var connection = await _connectionWrapper.GetConnectionAsync();

            var streamName = GetStreamName(aggregateRoot.Id);

            var firstEvent = aggregateRoot.Events.First();

            var version = firstEvent.AggregateVersion - 1;

            using var transaction = await connection.StartTransactionAsync(streamName, version);

            try
            {
                foreach (var @event in aggregateRoot.Events)
                {
                    var eventData = Map(@event);
                    await transaction.WriteAsync(eventData);
                }

                await transaction.CommitAsync();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task<TAggregateRoot> RehydrateAsync(TKey key)
        {
            var connection = await _connectionWrapper.GetConnectionAsync();

            var streamName = GetStreamName(key);

            var events = new List<IEvent<TKey>>();

            StreamEventsSlice currentSlice;
            long nextSliceStart = StreamPosition.Start;
            do
            {
                currentSlice = await connection.ReadStreamEventsForwardAsync(
                    streamName,
                    nextSliceStart,
                    200,
                    false);

                nextSliceStart = currentSlice.NextEventNumber;

                events.AddRange(currentSlice.Events.Select(Map));
            } while (!currentSlice.IsEndOfStream);

            var result = AggregateRoot<TAggregateRoot, TKey>.Create(
                events.OrderBy(
                    e => e.AggregateVersion));

            return result;
        }

        private string GetStreamName(TKey aggregateId)
        {
            var streamName = $"{_streamBaseName}_{aggregateId}";
            return streamName;
        }

        private IEvent<TKey> Map(ResolvedEvent resolvedEvent)
        {
            var meta = JsonSerializer.Deserialize<EventMeta>(resolvedEvent.Event.Metadata);
            return _eventDeserializer.Deserialize<TKey>(meta.EventType, resolvedEvent.Event.Data);
        }

        private static EventData Map(IEvent<TKey> @event)
        {
            var json = JsonSerializer.Serialize((dynamic) @event);
            var data = Encoding.UTF8.GetBytes(json);

            var eventType = @event.GetType();
            var meta = new EventMeta
            {
                EventType = eventType.AssemblyQualifiedName
            };
            var metaJson = JsonSerializer.Serialize(meta);
            var metadata = Encoding.UTF8.GetBytes(metaJson);

            var eventPayload = new EventData(
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