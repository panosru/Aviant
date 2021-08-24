namespace Aviant.DDD.Application.ApplicationEvents
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Timing;
    using MediatR;

    public sealed class ApplicationEventDispatcher : IApplicationEventDispatcher
    {
        private readonly IMediator _mediator;

        public ApplicationEventDispatcher(IMediator mediator) => _mediator = mediator;

        private List<IApplicationEvent> PreCommitEvents { get; } = new();

        private List<IApplicationEvent> PostCommitEvents { get; } = new();

        #region IApplicationEventDispatcher Members

        public void AddPreCommitEvent(IApplicationEvent applicationEvent)
        {
            applicationEvent.Occured = Clock.Now;
            PreCommitEvents.Add(applicationEvent);
        }

        public void AddPostCommitEvent(IApplicationEvent applicationEvent)
        {
            applicationEvent.Occured = Clock.Now;
            PostCommitEvents.Add(applicationEvent);
        }

        public async Task FirePreCommitEventsAsync(CancellationToken cancellationToken = default)
        {
            foreach (var notification in PreCommitEvents.ToList())
            {
                await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
                RemovePreCommitEvent(notification);
            }
        }

        public async Task FirePostCommitEventsAsync(CancellationToken cancellationToken = default)
        {
            foreach (var notification in PostCommitEvents.ToList())
            {
                await _mediator.Publish(notification, cancellationToken).ConfigureAwait(false);
                RemovePostCommitEvent(notification);
            }
        }

        public List<IApplicationEvent> GetPreCommitEvents() => PreCommitEvents;

        public List<IApplicationEvent> GetPostCommitEvents() => PostCommitEvents;

        public void RemovePreCommitEvent(IApplicationEvent applicationEvent)
        {
            PreCommitEvents.Remove(applicationEvent);
        }

        public void RemovePostCommitEvent(IApplicationEvent applicationEvent)
        {
            PostCommitEvents.Remove(applicationEvent);
        }

        #endregion
    }
}