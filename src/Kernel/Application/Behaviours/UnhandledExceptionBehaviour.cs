using MediatR;
using Serilog;

namespace Aviant.Application.Behaviours;

public sealed class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger = Log.Logger.ForContext<UnhandledExceptionBehaviour<TRequest, TResponse>>();

    #region IPipelineBehavior<TRequest,TResponse> Members

    public async Task<TResponse> Handle(
        TRequest          request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.Error(
                ex,
                "Unhandled Exception for Request {Name} {@Request}",
                requestName,
                request);

            throw;
        }
    }

    #endregion
}
