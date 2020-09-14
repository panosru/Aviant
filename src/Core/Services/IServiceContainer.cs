namespace Aviant.DDD.Core.Services
{
    #region

    using System;

    #endregion

    public interface IServiceContainer
    {
        T GetService<T>(Type type);
    }
}