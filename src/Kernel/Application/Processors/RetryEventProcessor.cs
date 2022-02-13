namespace Aviant.Application.Processors;

using Core.Services;
using MediatR;
using Polly;

public sealed class RetryEventProcessor<TNotification>
    : INotificationHandler<TNotification>
    where TNotification : INotification
{
    private readonly INotificationHandler<TNotification> _inner;

    private readonly IAsyncPolicy? _retryPolicy;

    public RetryEventProcessor(INotificationHandler<TNotification> inner)
    {
        _inner = inner;

        if (_inner is IRetry handler)
            _retryPolicy = handler.RetryPolicy();
    }

    #region INotificationHandler<TNotification> Members

    public Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        return _retryPolicy?.ExecuteAsync(
                   () =>
                       _inner.Handle(notification, cancellationToken))
            ?? throw new NullReferenceException(nameof(_retryPolicy));
    }

    #endregion
}
