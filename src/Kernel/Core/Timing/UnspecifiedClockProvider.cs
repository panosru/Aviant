namespace Aviant.Core.Timing;

public sealed class UnspecifiedClockProvider : IClockProvider
{
    public DateTime Now => DateTime.Now;

    public DateTimeKind Kind => DateTimeKind.Unspecified;

    public bool SupportsMultipleTimezone => false;

    public DateTime Normalize(DateTime dateTime) => dateTime;

    internal UnspecifiedClockProvider()
    { }
}
