namespace Aviant.DDD.Application.Services
{
    #region

    using System;
    using Domain.Services;
    using Microsoft.AspNetCore.Http;

    #endregion

    public class HttpContextServiceProviderProxy : IServiceContainer
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpContextServiceProviderProxy(IHttpContextAccessor contextAccessor) =>
            _contextAccessor = contextAccessor;

        #region IServiceContainer Members

        public T GetService<T>(Type type) => (T) _contextAccessor.HttpContext.RequestServices.GetService(type);

        #endregion
    }
}