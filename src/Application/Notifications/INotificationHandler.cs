namespace Aviant.DDD.Application.Notifications
{
    #region

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface INotificationHandler<in TNotification> : MediatR.INotificationHandler<TNotification>
        where TNotification : INotification
    {
        /// <summary>
        ///     Handles an notification
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        new Task Handle(TNotification notification, CancellationToken cancellationToken);
    }
}