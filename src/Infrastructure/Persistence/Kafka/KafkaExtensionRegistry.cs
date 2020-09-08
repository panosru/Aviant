namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using Domain.Aggregates;
    using Domain.EventBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class KafkaExtensionRegistry
    {
        public static IServiceCollection AddKafkaEventProducer<TAggregateRoot, TAggregateId>(
            this IServiceCollection services,
            EventConsumerConfig     configuration)
            where TAggregateRoot : class, IAggregateRoot<TAggregateId>
            where TAggregateId : class, IAggregateId
        {
            return services.AddSingleton<IEventProducer<TAggregateRoot, TAggregateId>>(
                ctx =>
                {
                    var logger = ctx.GetRequiredService<ILogger<EventProducer<TAggregateRoot, TAggregateId>>>();

                    return new EventProducer<TAggregateRoot, TAggregateId>(
                        configuration.TopicBaseName,
                        configuration.KafkaConnectionString,
                        logger);
                });
        }
    }
}