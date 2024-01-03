using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Aviant.Application.Interceptors;

public static class ServicesExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="interceptors"></param>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    public static void RegisterProxied<TInterface, TImplementation>(
        this   IServiceCollection services,
        params Type[]             interceptors)
        where TInterface : class
        where TImplementation : class, TInterface =>
        RegisterProxied<TInterface, TImplementation>(
            services,
            ProxyInterceptorLifetime.Transient,
            interceptors);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <param name="interceptors"></param>
    /// <typeparam name="TInterface"></typeparam>
    /// <typeparam name="TImplementation"></typeparam>
    public static void RegisterProxied<TInterface, TImplementation>(
        this   IServiceCollection services,
        ProxyInterceptorLifetime lifetime = ProxyInterceptorLifetime.Transient,
        params Type[]             interceptors)
        where TInterface : class
        where TImplementation : class, TInterface =>
        RegisterProxied(
            services,
            typeof(TInterface),
            typeof(TImplementation),
            lifetime,
            interceptors);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="targetInterface"></param>
    /// <param name="targetImplementation"></param>
    /// <param name="lifetime"></param>
    /// <param name="interceptors"></param>
    public static void RegisterProxied(
        this IServiceCollection  services,
        Type targetInterface,
        Type targetImplementation,
        ProxyInterceptorLifetime lifetime = ProxyInterceptorLifetime.Transient,
        params Type[]            interceptors)
    {
        // Register the Proxy Generator if not already registered
        services.TryAddSingleton<IProxyGenerator, ProxyGenerator>();

        // If interceptors are not provided, use the default interceptor
        if (interceptors.Length == 0)
            interceptors = new[] { typeof(IAsyncInterceptor) };

        switch (lifetime)
        {
            case ProxyInterceptorLifetime.Scoped:
                // Register the underlying classes
                services.TryAddScoped(targetImplementation);

                services.TryAddScoped(
                    targetInterface,
                    provider => ImplementProxy(
                        targetInterface,
                        targetImplementation,
                        provider,
                        interceptors));
                break;

            case ProxyInterceptorLifetime.Singleton:
                // Register the underlying classes
                services.TryAddSingleton(targetImplementation);

                services.TryAddSingleton(
                    targetInterface,
                    provider => ImplementProxy(
                        targetInterface,
                        targetImplementation,
                        provider,
                        interceptors));
                break;

            case ProxyInterceptorLifetime.Transient:
            default:
                // Register the underlying classes
                services.TryAddTransient(targetImplementation);

                services.TryAddTransient(
                    targetInterface,
                    provider => ImplementProxy(
                        targetInterface,
                        targetImplementation,
                        provider,
                        interceptors));
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetInterface"></param>
    /// <param name="targetImplementation"></param>
    /// <param name="provider"></param>
    /// <param name="interceptors"></param>
    /// <returns></returns>
    private static object ImplementProxy(
        Type targetInterface,
        Type targetImplementation,
        IServiceProvider provider,
        params Type[]    interceptors)
    {
        // Get an instance of the Castle Proxy Generator
        var proxyGenerator = provider.GetRequiredService<IProxyGenerator>();

        // Have DI build out an instance of the class that has methods that are intercepted
        // This is an uncached instance of the class
        var implementation = provider.GetRequiredService(targetImplementation);

        // Get all the registered interceptors
        var selectedInterceptors = provider.GetServices<IAsyncInterceptor>()
            // Select only the interceptors that implement the IAsyncInterceptor interface
           .Where(i => interceptors.ToList().Exists(o => o.IsInstanceOfType(i)))
            // Order the interceptors by the order they are registered
           .ToArray();

        // Castle Proxy build out a proxy object that implements the instance.
        // This proxy object is what will then get injected into the class tha has a dependency on TIInterface
        return proxyGenerator.CreateInterfaceProxyWithTargetInterface(
            targetInterface,
            implementation,
            selectedInterceptors);
    }
}
