namespace Aviant.Application.Services;

using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

public sealed class HttpContextServiceProviderProxy : IServiceContainer
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextServiceProviderProxy(IHttpContextAccessor contextAccessor) =>
        _contextAccessor = contextAccessor;

    #region IServiceContainer Members

    public object GetRequiredService(Type type) =>
        _contextAccessor.HttpContext?.RequestServices.GetRequiredService(type)
     ?? throw new NullReferenceException(type.FullName);

    public T GetRequiredService<T>(Type type) =>
        (T)(_contextAccessor.HttpContext?.RequestServices.GetRequiredService(type)
         ?? throw new NullReferenceException(type.FullName));

    public object GetService(Type type) =>
        _contextAccessor.HttpContext?.RequestServices.GetService(type)
     ?? throw new NullReferenceException(type.FullName);

    public T GetService<T>(Type type) =>
        (T)(_contextAccessor.HttpContext?.RequestServices.GetService(type)
         ?? throw new NullReferenceException(type.FullName));

    #endregion
}
