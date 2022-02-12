namespace Aviant.Foundation.Application.ApplicationEvents;

using Core.Timing;

public abstract record ApplicationEvent : IApplicationEvent
{
    protected ApplicationEvent() => Occured = Clock.Now;

    #region IApplicationEvent Members

    public DateTime Occured { get; set; }

    #endregion
}
