using Polly;

namespace Aviant.Application.ApplicationEvents;

public abstract class ApplicationEventHandler<TApplicationEvent>
    : IApplicationEventHandler<TApplicationEvent>
    where TApplicationEvent : IApplicationEvent
{
    #region IApplicationEventHandler<TApplicationEvent> Members

    public abstract Task Handle(TApplicationEvent @event, CancellationToken cancellationToken);

    public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

    #endregion
}
