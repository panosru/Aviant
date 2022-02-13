namespace Aviant.Core.Services;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceLocator
{
    private static IServiceContainer? _serviceContainer;

    public static IServiceContainer ServiceContainer =>
        _serviceContainer ?? throw new NullReferenceException(typeof(ServiceLocator).FullName);

    public static void Initialise(IServiceProvider serviceProvider)
    {
        _serviceContainer = serviceProvider.GetRequiredService<IServiceContainer>();
    }
}
