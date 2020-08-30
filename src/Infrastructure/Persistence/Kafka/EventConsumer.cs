namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using System;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Confluent.Kafka;
    using Domain.Aggregates;
    using Domain.EventBus;
    using Domain.Events;
    using Domain.Services;
    using Microsoft.Extensions.Logging;

    public class EventConsumer<TAggregateRoot, TKey> : IDisposable, IEventConsumer<TAggregateRoot, TKey>
        where TAggregateRoot : IAggregateRoot<TKey>
    {
        public delegate void ConsumerStoppedHandler(object sender);

        public delegate void ExceptionThrownHandler(object sender, Exception e);

        private readonly IEventDeserializer _eventDeserializer;
        private readonly ILogger<EventConsumer<TAggregateRoot, TKey>> _logger;
        private IConsumer<TKey, string> _eventConsumer;

        public EventConsumer(
            IEventDeserializer eventDeserializer,
            EventConsumerConfig config,
            ILogger<EventConsumer<TAggregateRoot, TKey>> logger)
        {
            _eventDeserializer = eventDeserializer;
            _logger = logger;

            var aggregateType = typeof(TAggregateRoot);

            var consumerConfig = new ConsumerConfig
            {
                GroupId = config.ConsumerGroup,
                BootstrapServers = config.KafkaConnectionString,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnablePartitionEof = true
            };

            var consumerBuilder = new ConsumerBuilder<TKey, string>(consumerConfig);
            var keyDeserializerFactory = new KeyDeserializerFactory();
            consumerBuilder.SetKeyDeserializer(keyDeserializerFactory.Create<TKey>());

            _eventConsumer = consumerBuilder.Build();

            var topicName = $"{config.TopicBaseName}-{aggregateType.Name}";
            _eventConsumer.Subscribe(topicName);
        }

        public void Dispose()
        {
            _eventConsumer?.Dispose();
            _eventConsumer = null;
        }

        public Task ConsumeAsync(CancellationToken stoppingToken)
        {
            return Task.Run(
                async () =>
                {
                    var topics = string.Join(",", _eventConsumer.Subscription);
                    _logger.LogInformation(
                        "started Kafka consumer {ConsumerName} on {ConsumerTopic}",
                        _eventConsumer.Name,
                        topics);

                    while (!stoppingToken.IsCancellationRequested)
                        try
                        {
                            var cr = _eventConsumer.Consume(stoppingToken);
                            if (cr.IsPartitionEOF)
                                continue;

                            var messageTypeHeader = cr.Message.Headers.First(h => h.Key == "type");
                            var eventType = Encoding.UTF8.GetString(messageTypeHeader.GetValueBytes());

                            var @event = _eventDeserializer.Deserialize<TKey>(eventType, cr.Message.Value);
                            if (null == @event)
                                throw new SerializationException(
                                    $"unable to deserialize notification {eventType} : {cr.Message.Value}");

                            await OnEventReceived(@event);
                        }
                        catch (OperationCanceledException ex)
                        {
                            _logger.LogWarning(
                                ex,
                                "consumer {ConsumerName} on {ConsumerTopic} was stopped: {StopReason}",
                                _eventConsumer.Name,
                                topics,
                                ex.Message);
                            OnConsumerStopped();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"an exception has occurred while consuming a message: {ex.Message}");
                            OnExceptionThrown(ex);
                        }
                },
                stoppingToken);
        }

        public event EventReceivedHandler<TKey> EventReceived;

        protected virtual Task OnEventReceived(IEvent<TKey> e)
        {
            var handler = EventReceived;
            return handler?.Invoke(this, e);
        }

        public event ExceptionThrownHandler ExceptionThrown;

        protected virtual void OnExceptionThrown(Exception e)
        {
            var handler = ExceptionThrown;
            handler?.Invoke(this, e);
        }

        public event ConsumerStoppedHandler ConsumerStopped;

        protected virtual void OnConsumerStopped()
        {
            var handler = ConsumerStopped;
            handler?.Invoke(this);
        }
    }
}