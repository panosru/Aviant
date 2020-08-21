namespace Aviant.DDD.Domain.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
        where TEvent : IEvent
    {
        /// <summary>
        ///     Handles an event
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        new Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}