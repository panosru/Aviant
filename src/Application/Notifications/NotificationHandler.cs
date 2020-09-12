namespace Aviant.DDD.Application.Notifications
{
    #region

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public abstract class NotificationHandler<TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        #region INotificationHandler<TNotification> Members

        public abstract Task Handle(TNotification notification, CancellationToken cancellationToken);

        #endregion
    }
}