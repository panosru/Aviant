namespace Aviant.DDD.Application.ApplicationEvents
{
    using System;
    using Core.Services;
    using Services;

    public abstract class ApplicationEvent : IApplicationEvent
    {
        #region IApplicationEvent Members

        public DateTime Occured { get; set; }

        protected ApplicationEvent() =>
            Occured = ServiceLocator.ServiceContainer.GetService<IDateTimeService>(
                    typeof(IDateTimeService))
               .Now(true);

        #endregion
    }
}