using System.Collections.ObjectModel;

namespace Aviant.Application.ApplicationEvents;

public interface IApplicationEventDispatcher
{
    #region Pre Commit Notifications

    public Collection<IApplicationEvent> GetPreCommitEvents();

    public void AddPreCommitEvent(IApplicationEvent applicationEvent);

    public void RemovePreCommitEvent(IApplicationEvent applicationEvent);

    public Task FirePreCommitEventsAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Post Commit Notifications

    public Collection<IApplicationEvent> GetPostCommitEvents();

    public void AddPostCommitEvent(IApplicationEvent applicationEvent);

    public void RemovePostCommitEvent(IApplicationEvent applicationEvent);

    public Task FirePostCommitEventsAsync(CancellationToken cancellationToken = default);

    #endregion
}
