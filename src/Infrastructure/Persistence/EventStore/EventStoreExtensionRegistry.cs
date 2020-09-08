namespace Aviant.DDD.Infrastructure.Persistence.EventStore
{
    using Application.Services;
    using Domain.Aggregates;
    using Domain.EventBus;
    using Domain.Persistence;
    using Domain.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class EventStoreExtensionRegistry
    {
        public static IServiceCollection AddEventsRepository<TAggregateRoot, TAggregateId>(
            this IServiceCollection services)
            where TAggregateRoot : class, IAggregateRoot<TAggregateId>
            where TAggregateId : class, IAggregateId
        {
            return services.AddSingleton<IEventsRepository<TAggregateRoot, TAggregateId>>(
                ctx =>
                {
                    var connectionWrapper = ctx.GetRequiredService<IEventStoreConnectionWrapper>();
                    var eventDeserializer = ctx.GetRequiredService<IEventDeserializer>();

                    return new EventsRepository<TAggregateRoot, TAggregateId>(connectionWrapper, eventDeserializer);
                });
        }

        public static IServiceCollection AddEventsService<TAggregateRoot, TAggregateId>(
            this IServiceCollection services)
            where TAggregateRoot : class, IAggregateRoot<TAggregateId>
            where TAggregateId : class, IAggregateId
        {
            return services.AddSingleton<IEventsService<TAggregateRoot, TAggregateId>>(
                ctx =>
                {
                    var eventsProducer = ctx.GetRequiredService<IEventProducer<TAggregateRoot, TAggregateId>>();
                    var eventsRepo     = ctx.GetRequiredService<IEventsRepository<TAggregateRoot, TAggregateId>>();

                    return new EventsService<TAggregateRoot, TAggregateId>(eventsRepo, eventsProducer);
                });
        }
    }
}