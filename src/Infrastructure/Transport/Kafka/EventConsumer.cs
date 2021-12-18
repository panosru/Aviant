namespace Aviant.DDD.Infrastructure.Transport.Kafka;

using System.Text;
using Confluent.Kafka;
using Core.Aggregates;
using Core.DomainEvents;
using Core.EventBus;
using Core.Services;
using Serilog;

public sealed class EventConsumer<TAggregate, TAggregateId, TDeserializer>
    : IDisposable, IEventConsumer<TAggregate, TAggregateId, TDeserializer>
    where TAggregate : IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
    where TDeserializer : class, IDeserializer<TAggregateId>, new()
{
    #region Delegates

    public delegate void ConsumerStoppedHandler(object sender);

    public delegate void ExceptionThrownHandler(object sender, Exception e);

    #endregion

    private readonly IEventDeserializer _eventDeserializer;

    private readonly ILogger _logger = Log.Logger.ForContext<EventConsumer<TAggregate, TAggregateId, TDeserializer>>();

    private IConsumer<TAggregateId, string> _eventConsumer;

    #pragma warning disable 8618
    public EventConsumer(
        IEventDeserializer  eventDeserializer,
        EventConsumerConfig config)
    {
        _eventDeserializer = eventDeserializer;

        var aggregateType = typeof(TAggregate);

        ConsumerConfig consumerConfig = new()
        {
            GroupId             = config.ConsumerGroup,
            BootstrapServers    = config.KafkaConnectionString,
            AutoOffsetReset     = AutoOffsetReset.Earliest,
            EnablePartitionEof  = true,
            BrokerAddressFamily = BrokerAddressFamily.V4
        };

        ConsumerBuilder<TAggregateId, string> consumerBuilder        = new(consumerConfig);
        KeyDeserializerFactory                keyDeserializerFactory = new();
        consumerBuilder.SetKeyDeserializer(keyDeserializerFactory.Create<TDeserializer, TAggregateId>());

        _eventConsumer = consumerBuilder.Build();

        var topicName = $"{config.TopicBaseName}-{aggregateType.Name}";
        _eventConsumer.Subscribe(topicName);
    }
    #pragma warning restore 8618

    #region IDisposable Members

    public void Dispose()
    {
        _eventConsumer.Dispose();
        _eventConsumer = null!;
    }

    #endregion

    #region IEventConsumer<TAggregate,TAggregateId,TDeserializer> Members

    // ReSharper disable once CognitiveComplexity
    public Task ConsumeAsync(CancellationToken cancellationToken)
    {
        return Task.Run(
            async () =>
            {
                var topics = string.Join(",", _eventConsumer.Subscription);

                _logger.Information(
                    "started Kafka consumer {ConsumerName} on {ConsumerTopic}",
                    _eventConsumer.Name,
                    topics);

                while (!cancellationToken.IsCancellationRequested)
                    try
                    {
                        ConsumeResult<TAggregateId, string> cr = _eventConsumer.Consume(cancellationToken);

                        if (cr.IsPartitionEOF)
                            continue;

                        var messageTypeHeader = cr.Message.Headers.First(h => h.Key == "type");
                        var eventType         = Encoding.UTF8.GetString(messageTypeHeader.GetValueBytes());

                        IDomainEvent<TAggregateId> @event =
                            _eventDeserializer.Deserialize<TAggregateId>(eventType, cr.Message.Value);

                        await OnEventReceivedAsync(@event, cancellationToken)
                           .ConfigureAwait(false);
                    }
                    catch (OperationCanceledException ex)
                    {
                        _logger.Warning(
                            ex,
                            "consumer {ConsumerName} on {ConsumerTopic} was stopped: {StopReason}",
                            _eventConsumer.Name,
                            topics,
                            ex.Message);
                        OnConsumerStopped();
                    }
                    catch (Exception ex)
                    {
                        _logger.Error(
                            ex,
                            "an exception has occurred while consuming a message: {Message}",
                            ex.Message);
                        OnExceptionThrown(ex);
                    }
            },
            cancellationToken);
    }

    public event EventReceivedHandlerAsync<TAggregateId> EventReceived;

    #endregion

    private Task OnEventReceivedAsync(
        IDomainEvent<TAggregateId> e,
        CancellationToken          cancellationToken)
    {
        EventReceivedHandlerAsync<TAggregateId> handlerAsync = EventReceived;

        return handlerAsync(this, e, cancellationToken)
            ?? throw new NullReferenceException(
                   typeof(EventConsumer<TAggregate, TAggregateId, TDeserializer>).FullName);
    }

    public event ExceptionThrownHandler ExceptionThrown;

    private void OnExceptionThrown(Exception e)
    {
        var handler = ExceptionThrown;
        handler(this, e);
    }

    public event ConsumerStoppedHandler ConsumerStopped;

    private void OnConsumerStopped()
    {
        var handler = ConsumerStopped;
        handler(this);
    }
}
