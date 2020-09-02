namespace Aviant.DDD.Application.Notifications
{
    using System;

    public interface INotification : MediatR.INotification
    {
        DateTime Occured { get; set; }
    }
}