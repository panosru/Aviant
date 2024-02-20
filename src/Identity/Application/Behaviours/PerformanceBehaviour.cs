using MediatR;

namespace Aviant.Application.Identity.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse>
    : Application.Behaviours.PerformanceBehaviour<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;

    private readonly IIdentityService _identityService;

    public PerformanceBehaviour(ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _currentUserService      = currentUserService;
        _identityService = identityService;
    }

    #region IPipelineBehavior<TRequest,TResponse> Members

    public new async Task<TResponse> Handle(
        TRequest                          request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken                 cancellationToken)
    {
        Timer.Start();
        var response = await next().ConfigureAwait(false);
        Timer.Stop();

        var elapsedMilliseconds = Timer.ElapsedMilliseconds;

        //TODO: Ability to configure the threshold
        if (500 >= elapsedMilliseconds)
            return response;

        var requestName = typeof(TRequest).Name;
        var userId      = _currentUserService.UserId;
        var username    = string.Empty;

        if (Guid.Empty != userId)
            username = await _identityService.GetUserNameAsync(userId, cancellationToken)
               .ConfigureAwait(false);

        Logger.Warning(
            "Long Running Request detected: {Name} ({ElapsedMilliseconds} milliseconds), UserId: {@UserId}, Username: {@Username}, Request: {@Request}",
            requestName,
            elapsedMilliseconds,
            userId,
            username,
            request);

        return response;
    }

    #endregion
}
