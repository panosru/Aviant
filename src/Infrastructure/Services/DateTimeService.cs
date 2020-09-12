namespace Aviant.DDD.Infrastructure.Services
{
    #region

    using System;
    using Application.Services;

    #endregion

    public class DateTimeService
        : IDateTimeService
    {
        #region IDateTimeService Members

        public DateTime Now(bool utc = false) => utc
            ? DateTime.UtcNow
            : DateTime.Now;

        #endregion
    }
}