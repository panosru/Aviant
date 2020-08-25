namespace Aviant.DDD.Application.Notifications
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class NotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        public abstract Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}