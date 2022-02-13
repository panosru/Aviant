namespace Aviant.Infrastructure.EventSourcing.Transport.Kafka;

using Core.EventSourcing.Aggregates;
using Core.EventSourcing.EventBus;
using Microsoft.Extensions.DependencyInjection;

public static class KafkaExtensionRegistry
{
    public static IServiceCollection AddKafkaEventProducer<TAggregate, TAggregateId>(
        this IServiceCollection services,
        EventsProducerConfig     configuration)
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        return services.AddSingleton<IEventProducer<TAggregate, TAggregateId>>(
            _ => new EventProducer<TAggregate, TAggregateId>(
                configuration.TopicName,
                configuration.KafkaConnectionString));
    }
}
