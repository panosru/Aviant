namespace Aviant.Application.Identity.Behaviours;

using MediatR;

public class PerformanceBehaviour<TRequest, TResponse>
    : Application.Behaviours.PerformanceBehaviour<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICurrentUserService _currentUserService;

    private readonly IIdentityService _identityIdentityService;

    public PerformanceBehaviour(ICurrentUserService currentUserService, IIdentityService identityIdentityService)
    {
        _currentUserService      = currentUserService;
        _identityIdentityService = identityIdentityService;
    }

    #region IPipelineBehavior<TRequest,TResponse> Members

    public new async Task<TResponse> Handle(
        TRequest                          request,
        CancellationToken                 cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        Timer.Start();
        var response = await next().ConfigureAwait(false);
        Timer.Stop();

        var elapsedMilliseconds = Timer.ElapsedMilliseconds;

        if (500 >= elapsedMilliseconds)
            return response;

        var requestName = typeof(TRequest).Name;
        var userId      = _currentUserService.UserId;
        var username    = string.Empty;

        if (Guid.Empty != userId)
            username = await _identityIdentityService.GetUserNameAsync(userId, cancellationToken)
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
