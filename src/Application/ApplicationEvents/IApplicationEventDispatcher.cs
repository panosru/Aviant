namespace Aviant.DDD.Application.ApplicationEvents
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IApplicationEventDispatcher
    {
        #region Pre Commit Notifications

        public List<IApplicationEvent> GetPreCommitEvents();

        public void AddPreCommitEvent(IApplicationEvent applicationEvent);

        public void RemovePreCommitEvent(IApplicationEvent applicationEvent);

        public Task FirePreCommitEventsAsync(CancellationToken cancellationToken = default);

        #endregion

        #region Post Commit Notifications

        public List<IApplicationEvent> GetPostCommitEvents();

        public void AddPostCommitEvent(IApplicationEvent applicationEvent);

        public void RemovePostCommitEvent(IApplicationEvent applicationEvent);

        public Task FirePostCommitEventsAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}