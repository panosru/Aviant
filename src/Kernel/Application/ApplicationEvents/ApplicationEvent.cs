using Aviant.Core.Timing;

namespace Aviant.Application.ApplicationEvents;

public abstract record ApplicationEvent : IApplicationEvent
{
    protected ApplicationEvent() => Occured = Clock.Now;

    #region IApplicationEvent Members

    public DateTime Occured { get; set; }

    #endregion
}
