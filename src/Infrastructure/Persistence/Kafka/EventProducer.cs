namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Core.Aggregates;
    using Core.EventBus;
    using Microsoft.Extensions.Logging;

    internal sealed class EventProducer<TAggregate, TAggregateId> : IEventProducer<TAggregate, TAggregateId>
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        private readonly ILogger<EventProducer<TAggregate, TAggregateId>> _logger;

        private readonly string _topicName;

        private IProducer<TAggregateId, string> _producer;

        public EventProducer(
            string                                           topicBaseName,
            string                                           kafkaConnString,
            ILogger<EventProducer<TAggregate, TAggregateId>> logger)
        {
            _logger = logger;

            var aggregateType = typeof(TAggregate);

            _topicName = $"{topicBaseName}-{aggregateType.Name}";

            ProducerConfig                        producerConfig  = new() { BootstrapServers = kafkaConnString };
            ProducerBuilder<TAggregateId, string> producerBuilder = new(producerConfig);
            producerBuilder.SetKeySerializer(new KeySerializer<TAggregateId>());
            _producer = producerBuilder.Build();
        }

        #region IEventProducer<TAggregate,TAggregateId> Members

        public void Dispose()
        {
            _producer.Dispose();
            _producer = null!;
        }

        public Task DispatchAsync(
            TAggregate        aggregate,
            CancellationToken cancellationToken = default)
        {
            if (aggregate is null)
                throw new ArgumentNullException(nameof(aggregate));

            return !aggregate.Events.Any()
                ? Task.CompletedTask
                : DispatchEventsAsync(aggregate, cancellationToken);
        }

        #endregion

        private async Task DispatchEventsAsync(
            TAggregate        aggregate,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
                "publishing " + aggregate.Events.Count + " events for {AggregateId} ...",
                aggregate.Id);

            foreach (Message<TAggregateId, string>? message in
                from @event in aggregate.Events
                let eventType = @event.GetType()
                let serialized = JsonSerializer.Serialize(@event, eventType)
                let headers = new Headers
                {
                    { "aggregate", Encoding.UTF8.GetBytes(@event.AggregateId.ToString()!) },
                    { "type", Encoding.UTF8.GetBytes(eventType.AssemblyQualifiedName!) }
                }
                select new Message<TAggregateId, string>
                {
                    Key     = @event.AggregateId,
                    Value   = serialized,
                    Headers = headers
                })
                await _producer.ProduceAsync(_topicName, message, cancellationToken)
                   .ConfigureAwait(false);

            aggregate.ClearEvents();
        }
    }
}