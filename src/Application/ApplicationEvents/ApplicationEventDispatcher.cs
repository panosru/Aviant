namespace Aviant.DDD.Application.ApplicationEvents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Services;

    public sealed class ApplicationEventDispatcher : IApplicationEventDispatcher
    {
        private readonly IDateTimeService _dateTimeService;

        private readonly IMediator _mediator;

        public ApplicationEventDispatcher(IMediator mediator, IDateTimeService dateTimeService)
        {
            _mediator        = mediator;
            _dateTimeService = dateTimeService;
        }

        private List<IApplicationEvent> PreCommitEvents { get; } = new List<IApplicationEvent>();

        private List<IApplicationEvent> PostCommitEvents { get; } = new List<IApplicationEvent>();

        #region IApplicationEventDispatcher Members

        public void AddPreCommitNotification(IApplicationEvent applicationEvent)
        {
            applicationEvent.Occured = _dateTimeService.Now(true);
            PreCommitEvents.Add(applicationEvent);
        }

        public void AddPostCommitNotification(IApplicationEvent applicationEvent)
        {
            applicationEvent.Occured = _dateTimeService.Now(true);
            PostCommitEvents.Add(applicationEvent);
        }

        public async Task FirePreCommitNotificationsAsync(CancellationToken cancellationToken = default)
        {
            foreach (var notification in PreCommitEvents.ToList())
            {
                await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
                RemovePreCommitNotification(notification);
            }
        }

        public async Task FirePostCommitNotificationsAsync(CancellationToken cancellationToken = default)
        {
            foreach (var notification in PostCommitEvents.ToList())
            {
                await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
                RemovePostCommitNotification(notification);
            }
        }

        public List<IApplicationEvent> GetPreCommitEvents() => PreCommitEvents;

        public List<IApplicationEvent> GetPostCommitNotifications() => PostCommitEvents;

        public void RemovePreCommitNotification(IApplicationEvent applicationEvent)
        {
            PreCommitEvents.Remove(applicationEvent);
        }

        public void RemovePostCommitNotification(IApplicationEvent applicationEvent)
        {
            PostCommitEvents.Remove(applicationEvent);
        }

        #endregion
    }
}