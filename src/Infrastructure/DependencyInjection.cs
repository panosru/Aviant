namespace Aviant.DDD.Infrastructure
{
    using Application.Notifications;
    using Application.Orchestration;
    using Application.Persistance;
    using Application.Services;
    using Domain.Messages;
    using Domain.Persistence;
    using Domain.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Persistance;
    using Services;

    public static class DependencyInjection
    {
        private static IServiceCollection RegisterInfrastructure<TDbContext>(
            this IServiceCollection services)
            where TDbContext : class, IApplicationDbContext
        {
            services.AddTransient<IDateTimeService, DateTimeService>();
            
            services.AddScoped<IOrchestrator, Orchestrator>();
            services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();
            services.AddScoped<IMessages, Messages>();
            services.AddScoped<INotificationDispatcher, NotificationDispatcher>();

            services.AddSingleton<IServiceContainer, HttpContextServiceProviderProxy>();
            
            return services;
        }
    }
}