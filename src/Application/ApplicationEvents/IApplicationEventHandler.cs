namespace Aviant.DDD.Application.ApplicationEvents
{
    using System.Threading;
    using System.Threading.Tasks;
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
        public Task Handle(TApplicationEvent @event, CancellationToken cancellationToken);
    }
}