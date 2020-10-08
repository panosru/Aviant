namespace Aviant.DDD.Application.ApplicationEvents
{
    using System;

    public abstract class ApplicationEvent : IApplicationEvent
    {
        #region IApplicationEvent Members

        public DateTime Occured { get; set; }

        #endregion
    }
}