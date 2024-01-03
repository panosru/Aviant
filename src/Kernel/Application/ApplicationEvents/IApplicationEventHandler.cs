using Aviant.Core.Services;
using MediatR;

namespace Aviant.Application.ApplicationEvents;

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
