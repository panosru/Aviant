namespace Aviant.EventSourcing.Infrastructure.Transport.Kafka;

using Core.Aggregates;
using Core.EventBus;
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
