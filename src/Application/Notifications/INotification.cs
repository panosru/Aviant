namespace Aviant.DDD.Application.Notifications
{
    #region

    using System;

    #endregion

    public interface INotification : MediatR.INotification
    {
        DateTime Occured { get; set; }
    }
}