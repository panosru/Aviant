namespace Aviant.DDD.Domain.Events
{
    using System;

    public abstract class Event : IEvent
    {
        public DateTime Occured { get; set; }
    }
}