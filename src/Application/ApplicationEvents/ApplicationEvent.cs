namespace Aviant.DDD.Application.ApplicationEvents
{
    using System;
    using Core.Timing;

    public abstract class ApplicationEvent : IApplicationEvent
    {
        protected ApplicationEvent() => Occured = Clock.Now;

        #region IApplicationEvent Members

        public DateTime Occured { get; set; }

        #endregion
    }
}
