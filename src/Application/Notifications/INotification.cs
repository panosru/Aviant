namespace Aviant.DDD.Application.Notifications
{
    using System;

    public interface INotification : MediatR.INotification
    {
        public DateTime Occured { get; set; }
    }
}