namespace Aviant.DDD.Infrastructure.Persistence.EventStore
{
    using Application.Services;
    using Domain.Aggregates;
    using Domain.EventBus;
    using Domain.Persistence;
    using Domain.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class DependencyInjection
    {
        public static IServiceCollection AddEventsRepository<TAggregateRoot, TKey>(this IServiceCollection services)
            where TAggregateRoot : class, IAggregateRoot<TKey>
        {
            return services.AddSingleton<IEventsRepository<TAggregateRoot, TKey>>(
                ctx =>
                {
                    var connectionWrapper = ctx.GetRequiredService<IEventStoreConnectionWrapper>();
                    var eventDeserializer = ctx.GetRequiredService<IEventDeserializer>();

                    return new EventsRepository<TAggregateRoot, TKey>(connectionWrapper, eventDeserializer);
                });
        }

        public static IServiceCollection AddEventsService<TAggregateRoot, TKey>(this IServiceCollection services)
            where TAggregateRoot : class, IAggregateRoot<TKey>
        {
            return services.AddSingleton<IEventsService<TAggregateRoot, TKey>>(
                ctx =>
                {
                    var eventsProducer = ctx.GetRequiredService<IEventProducer<TAggregateRoot, TKey>>();
                    var eventsRepo = ctx.GetRequiredService<IEventsRepository<TAggregateRoot, TKey>>();

                    return new EventsService<TAggregateRoot, TKey>(eventsRepo, eventsProducer);
                });
        }
    }
}