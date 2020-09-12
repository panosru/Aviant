namespace Aviant.DDD.Application.Services
{
    #region

    using System;

    #endregion

    public interface IDateTimeService
    {
        DateTime Now(bool utc = false);
    }
}