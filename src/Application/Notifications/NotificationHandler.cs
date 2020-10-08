namespace Aviant.DDD.Application.Notifications
{
    using System.Threading;
    using System.Threading.Tasks;
    using Polly;

    public abstract class NotificationHandler<TNotification>
        : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        #region INotificationHandler<TNotification> Members

        public abstract Task Handle(TNotification notification, CancellationToken cancellationToken);

        public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

        #endregion
    }
}