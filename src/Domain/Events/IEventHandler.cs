namespace Aviant.DDD.Domain.Events
{
    #region

    using EventBus;
    using MediatR;

    #endregion

    public interface IEventHandler<TEvent> : INotificationHandler<EventReceived<TEvent>>
    { }
}