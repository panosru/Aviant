namespace Aviant.DDD.Application.Notifications
{
    #region

    using System;

    #endregion

    public abstract class Notification : INotification
    {
        #region INotification Members

        public DateTime Occured { get; set; }

        #endregion
    }
}