namespace Aviant.Application.Identity.Behaviours;

public class LoggerBehaviour<TRequest> : Aviant.Application.Behaviours.LoggerBehaviour<TRequest>
    where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;

    private readonly IIdentityService _identityService;

    public LoggerBehaviour(ICurrentUserService currentUserService, IIdentityService identityService)
    {
        _currentUserService      = currentUserService;
        _identityService = identityService;
    }

    #region IRequestPreProcessor<TRequest> Members

    public override async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId      = _currentUserService.UserId;
        var username    = string.Empty;

        if (Guid.Empty != userId)
            username = await _identityService.GetUserNameAsync(userId, cancellationToken)
               .ConfigureAwait(false);

        Logger.Information(
            "Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName,
            userId,
            username,
            request);

        await Task.CompletedTask.ConfigureAwait(false);
    }

    #endregion
}
