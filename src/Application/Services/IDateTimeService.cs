namespace Aviant.DDD.Application.Services
{
    using System;

    public interface IDateTimeService
    {
        public DateTime Now(bool utc = false);
    }
}