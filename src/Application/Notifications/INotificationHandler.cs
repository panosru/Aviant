namespace Aviant.DDD.Application.Notifications
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface INotificationHandler<in TNotification> : MediatR.INotificationHandler<TNotification>
        where TNotification : INotification
    {
        /// <summary>
        ///     Handles an notification
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public new Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}