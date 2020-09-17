namespace Aviant.DDD.Application.Processors
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Polly;

    public class RetryProcessor<TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        private readonly INotificationHandler<TNotification> _inner;

        private readonly IAsyncPolicy _retryPolicy;

        public RetryProcessor(INotificationHandler<TNotification> inner)
        {
            _inner = inner; //TODO: check RetryDecorator doesn't get injected twice

            _retryPolicy = Policy
               .Handle<ArgumentOutOfRangeException>()
               .WaitAndRetryAsync(
                    3,
                    i => TimeSpan.FromSeconds(i));
        }

        #region INotificationHandler<TNotification> Members

        public Task Handle(TNotification notification, CancellationToken cancellationToken)
        {
            return _retryPolicy.ExecuteAsync(
                () =>
                    _inner.Handle(notification, cancellationToken));
        }

        #endregion
    }
}