namespace Aviant.DDD.Infrastructure.Services
{
    using System;
    using Application.Services;

    public class DateTimeService
        : IDateTimeService
    {
        public DateTime Now(bool utc = false)
        {
            return utc
                ? DateTime.UtcNow
                : DateTime.Now;
        }
    }
}