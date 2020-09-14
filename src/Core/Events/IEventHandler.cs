namespace Aviant.DDD.Core.Events
{
    #region

    using EventBus;
    using MediatR;

    #endregion

    public interface IEventHandler<TEvent> : INotificationHandler<EventReceived<TEvent>>
    { }
}