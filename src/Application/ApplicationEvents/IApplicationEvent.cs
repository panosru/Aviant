namespace Aviant.DDD.Application.ApplicationEvents
{
    using System;
    using MediatR;

    /// <inheritdoc />
    /// <summary>
    ///     Application Event Interface
    /// </summary>
    public interface IApplicationEvent : INotification
    {
        /// <summary>
        ///     The DateTime that the Application Event occurred.
        /// </summary>
        public DateTime Occured { get; set; }
    }
}