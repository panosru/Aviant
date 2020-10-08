namespace Aviant.DDD.Application.ApplicationEvents
{
    using System.Threading;
    using System.Threading.Tasks;
    using Polly;

    public abstract class ApplicationEventHandler<TNotification>
        : IApplicationEventHandler<TNotification>
        where TNotification : IApplicationEvent
    {
        #region IApplicationEventHandler<TNotification> Members

        public abstract Task Handle(TNotification @event, CancellationToken cancellationToken);

        public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

        #endregion
    }
}