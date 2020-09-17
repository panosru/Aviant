namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using Core.Aggregates;
    using Core.EventBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class KafkaExtensionRegistry
    {
        public static IServiceCollection AddKafkaEventProducer<TAggregate, TAggregateId>(
            this IServiceCollection services,
            EventConsumerConfig     configuration)
            where TAggregate : class, IAggregate<TAggregateId>
            where TAggregateId : class, IAggregateId
        {
            return services.AddSingleton<IEventProducer<TAggregate, TAggregateId>>(
                ctx =>
                {
                    var logger = ctx.GetRequiredService<ILogger<EventProducer<TAggregate, TAggregateId>>>();

                    return new EventProducer<TAggregate, TAggregateId>(
                        configuration.TopicBaseName,
                        configuration.KafkaConnectionString,
                        logger);
                });
        }
    }
}