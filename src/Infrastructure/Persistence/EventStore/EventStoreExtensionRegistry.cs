namespace Aviant.DDD.Infrastructure.Persistence.EventStore
{
    using Application.Services;
    using Core.Aggregates;
    using Core.EventBus;
    using Core.Persistence;
    using Core.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class EventStoreExtensionRegistry
    {
        public static IServiceCollection AddEventsRepository<TAggregate, TAggregateId>(this IServiceCollection services)
            where TAggregate : class, IAggregate<TAggregateId>
            where TAggregateId : class, IAggregateId
        {
            return services.AddSingleton<IEventsRepository<TAggregate, TAggregateId>>(
                ctx =>
                {
                    var connectionWrapper = ctx.GetRequiredService<IEventStoreConnectionWrapper>();
                    var eventDeserializer = ctx.GetRequiredService<IEventDeserializer>();

                    return new EventsRepository<TAggregate, TAggregateId>(connectionWrapper, eventDeserializer);
                });
        }

        public static IServiceCollection AddEventsService<TAggregate, TAggregateId>(this IServiceCollection services)
            where TAggregate : class, IAggregate<TAggregateId>
            where TAggregateId : class, IAggregateId
        {
            return services.AddSingleton<IEventsService<TAggregate, TAggregateId>>(
                ctx =>
                {
                    var eventsProducer = ctx.GetRequiredService<IEventProducer<TAggregate, TAggregateId>>();
                    var eventsRepo     = ctx.GetRequiredService<IEventsRepository<TAggregate, TAggregateId>>();

                    return new EventsService<TAggregate, TAggregateId>(eventsRepo, eventsProducer);
                });
        }
    }
}
