namespace Aviant.Core.Timing;

public static class ClockProviders
{
    internal static UnspecifiedClockProvider Unspecified { get; } = new();

    public static LocalClockProvider Local { get; } = new();

    public static UtcClockProvider Utc { get; } = new();
}
