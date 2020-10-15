namespace Aviant.DDD.Application.ApplicationEvents
{
    using System;
    using Core.Services;
    using Services;

    public abstract class ApplicationEvent : IApplicationEvent
    {
        protected ApplicationEvent() =>
            Occured = ServiceLocator.ServiceContainer.GetService<IDateTimeService>(
                    typeof(IDateTimeService))
               .Now(true);

        #region IApplicationEvent Members

        public DateTime Occured { get; set; }

        #endregion
    }
}