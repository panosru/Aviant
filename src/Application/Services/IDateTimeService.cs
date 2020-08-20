namespace Aviant.DDD.Application.Services
{
    using System;

    public interface IDateTimeService
    {
        DateTime Now(bool utc = false);
    }
}