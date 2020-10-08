namespace Aviant.DDD.Application.ApplicationEvents
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Services;
    using MediatR;

    internal interface IApplicationEventHandler<in TNotification>
        : INotificationHandler<TNotification>,
          IRetry
        where TNotification : IApplicationEvent
    {
        /// <summary>
        ///     Handles an applicationEvent
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public new Task Handle(TNotification @event, CancellationToken cancellationToken);
    }
}