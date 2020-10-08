namespace Aviant.DDD.Application.ApplicationEvents
{
    using System;
    using MediatR;

    public interface IApplicationEvent : INotification
    {
        public DateTime Occured { get; set; }
    }
}