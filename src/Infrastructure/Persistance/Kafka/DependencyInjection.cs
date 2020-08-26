namespace Aviant.DDD.Infrastructure.Persistance.Kafka
{
    using Domain.Aggregates;
    using Domain.EventBus;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class DependencyInjection
    {
        public static IServiceCollection AddKafkaEventProducer<TA, TK>(
            this IServiceCollection services, 
            EventConsumerConfig configuration)
            where TA : class, IAggregateRoot<TK>
        {
            return services.AddSingleton<IEventProducer<TA, TK>>(ctx =>
            {
                var logger = ctx.GetRequiredService<ILogger<EventProducer<TA, TK>>>();
                return new EventProducer<TA, TK>(
                    configuration.TopicBaseName,
                    configuration.KafkaConnectionString, 
                    logger);
            });
        }
    }
}