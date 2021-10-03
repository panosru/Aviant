namespace Aviant.DDD.Application.Behaviours
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Serilog;

    public sealed class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger _logger = Log.Logger.ForContext<UnhandledExceptionBehaviour<TRequest, TResponse>>();

        #region IPipelineBehavior<TRequest,TResponse> Members

        public async Task<TResponse> Handle(
            TRequest                          request,
            CancellationToken                 cancellationToken,
            RequestHandlerDelegate<TResponse> next)
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
}
