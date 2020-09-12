namespace Aviant.DDD.Application.Behaviours
{
    #region

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Identity;
    using MediatR.Pipeline;
    using Microsoft.Extensions.Logging;

    #endregion

    public class LoggerBehaviour<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        private readonly ICurrentUserService _currentUserService;

        private readonly IIdentityService _identityIdentityService;

        private readonly ILogger _logger;

        public LoggerBehaviour(
            ILogger<TRequest>   logger,
            ICurrentUserService currentUserService,
            IIdentityService    identityIdentityService)
        {
            _logger                  = logger;
            _currentUserService      = currentUserService;
            _identityIdentityService = identityIdentityService;
        }

        #region IRequestPreProcessor<TRequest> Members

        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId      = _currentUserService.UserId;
            var username    = string.Empty;

            if (Guid.Empty != userId)
                username = await _identityIdentityService.GetUserNameAsync(userId);

            _logger.LogInformation(
                "Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName,
                userId,
                username,
                request);

            await Task.CompletedTask;
        }

        #endregion
    }
}