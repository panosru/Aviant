namespace Aviant.DDD.Infrastructure.Persistence.EventStore
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Core.Aggregates;
    using Core.Events;
    using Core.Persistence;
    using Core.Services;
    using global::EventStore.ClientAPI;

    #endregion

    public class EventsRepository<TAggregate, TAggregateId> : IEventsRepository<TAggregate, TAggregateId>
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

        public async Task AppendAsync(TAggregate aggregate)
        {
            if (aggregate is null)
                throw new ArgumentNullException(nameof(aggregate));

            if (!aggregate.Events.Any())
                return;

            var connection = await _connectionWrapper.GetConnectionAsync();

            var streamName = GetStreamName(aggregate.Id);

            IEvent<TAggregateId> firstEvent = aggregate.Events.First();

            var version = firstEvent.AggregateVersion - 1;

            using var transaction = await connection.StartTransactionAsync(streamName, version);

            try
            {
                foreach (IEvent<TAggregateId> @event in aggregate.Events)
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

        public async Task<TAggregate> RehydrateAsync(TAggregateId aggregateId)
        {
            var connection = await _connectionWrapper.GetConnectionAsync();

            var streamName = GetStreamName(aggregateId);

            var events = new List<IEvent<TAggregateId>>();

            StreamEventsSlice currentSlice;
            long              nextSliceStart = StreamPosition.Start;

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

            var result = Aggregate<TAggregate, TAggregateId>.Create(
                events.OrderBy(
                    e => e.AggregateVersion));

            return result;
        }

        #endregion

        private string GetStreamName(TAggregateId aggregateId)
        {
            var streamName = $"{_streamBaseName}_{aggregateId}";

            return streamName;
        }

        private IEvent<TAggregateId> Map(ResolvedEvent resolvedEvent)
        {
            var meta = JsonSerializer.Deserialize<EventMeta>(resolvedEvent.Event.Metadata);

            return _eventDeserializer.Deserialize<TAggregateId>(meta.EventType, resolvedEvent.Event.Data);
        }

        private static EventData Map(IEvent<TAggregateId> @event)
        {
            var json = JsonSerializer.Serialize((dynamic) @event);
            var data = Encoding.UTF8.GetBytes(json);

            var eventType = @event.GetType();

            var meta = new EventMeta
            {
                EventType = eventType.AssemblyQualifiedName
            };
            var    metaJson = JsonSerializer.Serialize(meta);
            byte[] metadata = Encoding.UTF8.GetBytes(metaJson);

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