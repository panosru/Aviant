using MediatR;

namespace Aviant.Application.ApplicationEvents;

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
