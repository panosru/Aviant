namespace Aviant.DDD.Infrastructure.Services
{
    using System;
    using Application.Services;

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