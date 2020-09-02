namespace Aviant.DDD.Application.Notifications
{
    using System;

    public abstract class Notification : INotification
    {
    #region INotification Members

        public DateTime Occured { get; set; }

    #endregion
    }
}