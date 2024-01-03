using System.Diagnostics;
using MediatR;
using Serilog;

namespace Aviant.Application.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    protected readonly ILogger Logger = Log.Logger.ForContext<PerformanceBehaviour<TRequest, TResponse>>();

    protected readonly Stopwatch Timer;

    public PerformanceBehaviour() => Timer = new Stopwatch();

    #region IPipelineBehavior<TRequest,TResponse> Members

    public async Task<TResponse> Handle(
        TRequest          request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        Timer.Start();
        var response = await next().ConfigureAwait(false);
        Timer.Stop();

        var elapsedMilliseconds = Timer.ElapsedMilliseconds;

        //TODO: Add a configuration option to change the threshold
        if (500 >= elapsedMilliseconds)
            return response;

        var requestName = typeof(TRequest).Name;

        Logger.Warning(
            "Long Running Request detected: {Name} ({ElapsedMilliseconds} milliseconds), Request: {@Request}",
            requestName,
            elapsedMilliseconds,
            request);

        return response;
    }

    #endregion
}
