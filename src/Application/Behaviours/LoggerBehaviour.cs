namespace Aviant.DDD.Application.Behaviours
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Identity;
    using Microsoft.Extensions.Logging;
    using Processors;

    public sealed class LoggerBehaviour<TRequest> : RequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        private readonly ICurrentUserService _currentUserService;

        private readonly IIdentityService _identityIdentityService;

        private readonly ILogger<LoggerBehaviour<TRequest>> _logger;

        public LoggerBehaviour(
            ILogger<LoggerBehaviour<TRequest>>   logger,
            ICurrentUserService currentUserService,
            IIdentityService    identityIdentityService)
        {
            _logger                  = logger;
            _currentUserService      = currentUserService;
            _identityIdentityService = identityIdentityService;
        }

        #region IRequestPreProcessor<TRequest> Members

        public override async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId      = _currentUserService.UserId;
            var username    = string.Empty;

            if (Guid.Empty != userId)
                username = await _identityIdentityService.GetUserNameAsync(userId, cancellationToken)
                   .ConfigureAwait(false);

            _logger.LogInformation(
                "Request: {Name} {UserId} {UserName} {Request}",
                requestName,
                userId,
                username,
                request);

            await Task.CompletedTask.ConfigureAwait(false);
        }

        #endregion
    }
}