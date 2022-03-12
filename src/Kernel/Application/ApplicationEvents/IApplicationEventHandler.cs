namespace Aviant.Application.ApplicationEvents;

using Core.Services;
using MediatR;

internal interface IApplicationEventHandler<in TApplicationEvent>
    : INotificationHandler<TApplicationEvent>,
      IRetry
    where TApplicationEvent : IApplicationEvent
{
    /// <summary>
    ///     Handles an Application Event
    /// </summary>
    /// <param name="event"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public new Task Handle(TApplicationEvent @event, CancellationToken cancellationToken);
}