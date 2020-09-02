namespace Aviant.DDD.Infrastructure
{
    public static class DependencyInjection
    {
        // public static IServiceCollection RegisterInfrastructure<TDbContext>(
        //     this IServiceCollection services,
        //     string eventStoreConnectionString,
        //     EventConsumerConfig consumerConfig)
        //     where TDbContext : class, IApplicationDbContext
        // {
        //     services.AddTransient<IDateTimeService, DateTimeService>();
        //
        //     services.AddScoped<IOrchestrator, Orchestrator>();
        //     services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();
        //     services.AddScoped<IMessages, Messages>();
        //     services.AddScoped<INotificationDispatcher, NotificationDispatcher>();
        //
        //     // Event Store
        //     services.AddSingleton<IEventStoreConnectionWrapper>(
        //         ctx =>
        //         {
        //             var logger = ctx.GetRequiredService<ILogger<EventStoreConnectionWrapper>>();
        //             return new EventStoreConnectionWrapper(new Uri(eventStoreConnectionString), logger);
        //         });
        //
        //     // Kafka
        //     services.AddSingleton(consumerConfig)
        //         .AddSingleton(typeof(IEventConsumer<,>), typeof(EventConsumer<,,>));
        //
        //     services.AddSingleton<IServiceContainer, HttpContextServiceProviderProxy>();
        //
        //     return services;
        // }
    }
}