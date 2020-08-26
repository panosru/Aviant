namespace Aviant.DDD.Application.Orchestration
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Commands;
    using Domain.Messages;
    using Domain.Persistence;
    using MediatR;
    using Notifications;
    using Queries;

    public class Orchestrator : IOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IMessages _messages;
        private readonly INotificationDispatcher _notificationDispatcher;
        private readonly IUnitOfWork _unitOfWork;

        public Orchestrator(
            IUnitOfWork unitOfWork,
            IMessages messages,
            INotificationDispatcher notificationDispatcher,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _messages = messages;
            _notificationDispatcher = notificationDispatcher;
            _mediator = mediator;
        }

        public async Task<RequestResult> SendCommand<T>(ICommand<T> command)
        {
            var commandResponse = await _mediator.Send(command);

            // Fire pre/post events
            await _notificationDispatcher.FirePreCommitNotifications();

            if (_messages.HasMessages()) return new RequestResult(_messages.GetAll());

            var affectedRows = await _unitOfWork.Commit();

            if (-1 == affectedRows)
                return new RequestResult(
                    new List<string>
                    {
                        "An error occurred"
                    });

            // Fire post commit events
            await _notificationDispatcher.FirePostCommitNotifications();

            var isLazy = false;

            try
            {
                isLazy = typeof(Lazy<>) == commandResponse?.GetType().GetGenericTypeDefinition();
            }
            catch (Exception)
            {
                // ignored
            }

            return new RequestResult(
                isLazy
                    ? commandResponse?.GetType().GetProperty("Value")?.GetValue(commandResponse, null)
                    : commandResponse,
                affectedRows);
        }

        public async Task<RequestResult> SendQuery<T>(IQuery<T> query)
        {
            var commandResponse = await _mediator.Send(query);

            if (_messages.HasMessages()) return new RequestResult(_messages.GetAll());

            return new RequestResult(commandResponse);
        }
    }
}