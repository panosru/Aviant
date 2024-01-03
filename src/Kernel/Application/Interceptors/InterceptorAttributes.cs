using Castle.DynamicProxy;

namespace Aviant.Application.Interceptors;

public abstract class InterceptorBaseAttribute : Attribute;

public abstract class InterceptorBaseAttribute<TInterceptor> : InterceptorBaseAttribute
    where TInterceptor : class, IAsyncInterceptor
{
    public Type InterceptorType { get; } = typeof(TInterceptor);
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class InterceptorAttribute<TInterceptor> : InterceptorBaseAttribute<TInterceptor>
    where TInterceptor : class, IAsyncInterceptor
{
    // By default, the interceptor is applied to all methods
    // that are not marked with the [NoIntercept] attribute.
    public bool Explicit { get; set; }
}

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Event | AttributeTargets.Delegate)]
public sealed class NoInterceptAttribute : InterceptorBaseAttribute;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Event | AttributeTargets.Delegate, AllowMultiple = true)]
public sealed class NoInterceptAttribute<TInterceptor> : InterceptorBaseAttribute<TInterceptor>
    where TInterceptor : class, IAsyncInterceptor;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Event | AttributeTargets.Delegate)]
public sealed class InterceptAttribute : InterceptorBaseAttribute;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Event | AttributeTargets.Delegate, AllowMultiple = true)]
public sealed class InterceptAttribute<TInterceptor> : InterceptorBaseAttribute<TInterceptor>
    where TInterceptor : class, IAsyncInterceptor;
