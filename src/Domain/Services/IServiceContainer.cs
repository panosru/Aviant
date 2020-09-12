namespace Aviant.DDD.Domain.Services
{
    #region

    using System;

    #endregion

    public interface IServiceContainer
    {
        T GetService<T>(Type type);
    }
}