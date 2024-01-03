using Aviant.Application.Processors;
using Serilog;

namespace Aviant.Application.Behaviours;

public class LoggerBehaviour<TRequest> : RequestPreProcessor<TRequest>
    where TRequest : notnull
{
    protected readonly ILogger Logger = Log.Logger.ForContext<LoggerBehaviour<TRequest>>();

    #region IRequestPreProcessor<TRequest> Members

    public override async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        Logger.Information(
            "Request: {Name} {@Request}",
            requestName,
            request);

        await Task.CompletedTask.ConfigureAwait(false);
    }

    #endregion
}
