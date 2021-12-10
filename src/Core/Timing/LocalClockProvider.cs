namespace Aviant.DDD.Core.Timing;

/// <inheritdoc />
/// <summary>
/// Implements <see cref="T:Aviant.DDD.Core.Timing.IClockProvider" /> to work with local times.
/// </summary>
public sealed class LocalClockProvider : IClockProvider
{
    public DateTime Now => DateTime.Now;

    public DateTimeKind Kind => DateTimeKind.Local;

    public bool SupportsMultipleTimezone => false;

    public DateTime Normalize(DateTime dateTime)
    {
        return dateTime.Kind switch
        {
            DateTimeKind.Unspecified => DateTime.SpecifyKind(dateTime, DateTimeKind.Local),
            DateTimeKind.Utc         => dateTime.ToLocalTime(),
            _                        => dateTime
        };
    }

    internal LocalClockProvider()
    { }
}
