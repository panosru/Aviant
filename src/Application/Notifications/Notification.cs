namespace Aviant.DDD.Application.Notifications
{
    using System;

    public abstract class Notification : INotification
    {
        public DateTime Occured { get; set; }
    }
}