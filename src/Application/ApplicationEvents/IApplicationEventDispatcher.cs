namespace Aviant.DDD.Application.ApplicationEvents
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IApplicationEventDispatcher
    {
        #region Pre Commit Notifications

        public List<IApplicationEvent> GetPreCommitEvents();

        public void AddPreCommitNotification(IApplicationEvent applicationEvent);

        public void RemovePreCommitNotification(IApplicationEvent applicationEvent);

        public Task FirePreCommitNotificationsAsync(CancellationToken cancellationToken = default);

        #endregion

        #region Post Commit Notifications

        public List<IApplicationEvent> GetPostCommitNotifications();

        public void AddPostCommitNotification(IApplicationEvent applicationEvent);

        public void RemovePostCommitNotification(IApplicationEvent applicationEvent);

        public Task FirePostCommitNotificationsAsync(CancellationToken cancellationToken = default);

        #endregion
    }
}