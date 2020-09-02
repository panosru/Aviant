namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Domain.Aggregates;
    using Domain.EventBus;
    using Domain.Events;
    using Microsoft.Extensions.Logging;

    public class EventProducer<TAggregateRoot, TAggregateId> : IDisposable, IEventProducer<TAggregateRoot, TAggregateId>
        where TAggregateRoot : IAggregateRoot<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        private readonly ILogger<EventProducer<TAggregateRoot, TAggregateId>> _logger;

        private readonly string _topicName;

        private IProducer<TAggregateId, string> _producer;

        public EventProducer(
            string                                               topicBaseName,
            string                                               kafkaConnString,
            ILogger<EventProducer<TAggregateRoot, TAggregateId>> logger)
        {
            _logger = logger;

            var aggregateType = typeof(TAggregateRoot);

            _topicName = $"{topicBaseName}-{aggregateType.Name}";

            var producerConfig  = new ProducerConfig { BootstrapServers = kafkaConnString };
            var producerBuilder = new ProducerBuilder<TAggregateId, string>(producerConfig);
            producerBuilder.SetKeySerializer(new KeySerializer<TAggregateId>());
            _producer = producerBuilder.Build();
        }

    #region IDisposable Members

        public void Dispose()
        {
            _producer?.Dispose();
            _producer = null;
        }

    #endregion

    #region IEventProducer<TAggregateRoot,TAggregateId> Members

        public async Task DispatchAsync(TAggregateRoot aggregateRoot)
        {
            if (null == aggregateRoot)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            _logger.LogInformation(
                "publishing " + aggregateRoot.Events.Count + " events for {AggregateId} ...",
                aggregateRoot.Id);

            foreach (IEvent<TAggregateId> @event in aggregateRoot.Events)
            {
                var eventType = @event.GetType();

                var serialized = JsonSerializer.Serialize(@event, eventType);

                var headers = new Headers
                {
                    { "aggregate", Encoding.UTF8.GetBytes(@event.AggregateId.ToString()) },
                    { "type", Encoding.UTF8.GetBytes(eventType.AssemblyQualifiedName) }
                };

                var message = new Message<TAggregateId, string>
                {
                    Key     = @event.AggregateId,
                    Value   = serialized,
                    Headers = headers
                };

                await _producer.ProduceAsync(_topicName, message);
            }

            aggregateRoot.ClearEvents();
        }

    #endregion
    }
}