namespace Aviant.Foundation.Application.Processors;

using Core.Services;
using MediatR;
using Polly;

public sealed class RetryRequestProcessor<TRequest, TResponse>
    : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner;

    private readonly IAsyncPolicy? _retryPolicy;

    public RetryRequestProcessor(IRequestHandler<TRequest, TResponse> inner)
    {
        _inner = inner;

        if (_inner is IRetry handler)
            _retryPolicy = handler.RetryPolicy();
    }

    #region IRequestHandler<TRequest,TResponse> Members

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        return _retryPolicy?.ExecuteAsync(
                   () =>
                       _inner.Handle(request, cancellationToken))
            ?? throw new NullReferenceException(nameof(_retryPolicy));
    }

    #endregion
}
