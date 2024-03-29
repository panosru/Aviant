using MediatR;

namespace Aviant.Application.Processors;

public abstract class RequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    #region IRequestPostProcessor<TRequest,TResponse> Members

    public abstract Task Process(
        TRequest          request,
        TResponse         response,
        CancellationToken cancellationToken);

    #endregion
}
