namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using Core.Aggregates;
    using Core.EventBus;
    using Microsoft.Extensions.DependencyInjection;

    public static class KafkaExtensionRegistry
    {
        public static IServiceCollection AddKafkaEventProducer<TAggregate, TAggregateId>(
            this IServiceCollection services,
            EventConsumerConfig     configuration)
            where TAggregate : class, IAggregate<TAggregateId>
            where TAggregateId : class, IAggregateId
        {
            return services.AddSingleton<IEventProducer<TAggregate, TAggregateId>>(
                _ => new EventProducer<TAggregate, TAggregateId>(
                    configuration.TopicBaseName,
                    configuration.KafkaConnectionString));
        }
    }
}
