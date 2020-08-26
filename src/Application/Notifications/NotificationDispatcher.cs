namespace Aviant.DDD.Application.Notifications
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediatR;
    using Services;

    public class NotificationDispatcher : INotificationDispatcher
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly IMediator _mediator;

        public NotificationDispatcher(IMediator mediator, IDateTimeService dateTimeService)
        {
            _mediator = mediator;
            _dateTimeService = dateTimeService;
        }

        private List<INotification> PreCommitEvents { get; } = new List<INotification>();
        private List<INotification> PostCommitEvents { get; } = new List<INotification>();

        public void AddPreCommitNotification(INotification notification)
        {
            notification.Occured = _dateTimeService.Now(true);
            PreCommitEvents.Add(notification);
        }

        public void AddPostCommitNotification(INotification notification)
        {
            notification.Occured = _dateTimeService.Now(true);
            PostCommitEvents.Add(notification);
        }

        public async Task FirePreCommitNotifications()
        {
            foreach (INotification notification in PreCommitEvents.ToList())
            {
                await _mediator.Publish(notification).ConfigureAwait(false);
                RemovePreCommitNotification(notification);
            }
        }

        public async Task FirePostCommitNotifications()
        {
            foreach (INotification notification in PostCommitEvents.ToList())
            {
                await _mediator.Publish(notification).ConfigureAwait(false);
                RemovePostCommitNotification(notification);
            }
        }

        public List<INotification> GetPreCommitEvents()
        {
            return PreCommitEvents;
        }

        public List<INotification> GetPostCommitNotifications()
        {
            return PostCommitEvents;
        }

        public void RemovePreCommitNotification(INotification notification)
        {
            PreCommitEvents.Remove(notification);
        }

        public void RemovePostCommitNotification(INotification notification)
        {
            PostCommitEvents.Remove(notification);
        }
    }
}