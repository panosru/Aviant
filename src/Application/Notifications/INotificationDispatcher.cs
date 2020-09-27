namespace Aviant.DDD.Application.Notifications
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface INotificationDispatcher
    {
        #region Pre Commit Notifications

        public List<INotification> GetPreCommitEvents();

        public void AddPreCommitNotification(INotification notification);

        public void RemovePreCommitNotification(INotification notification);

        public Task FirePreCommitNotifications(CancellationToken cancellationToken = default);

        #endregion

        #region Post Commit Notifications

        public List<INotification> GetPostCommitNotifications();

        public void AddPostCommitNotification(INotification notification);

        public void RemovePostCommitNotification(INotification notification);

        public Task FirePostCommitNotifications(CancellationToken cancellationToken = default);

        #endregion
    }
}