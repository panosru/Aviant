namespace Aviant.Foundation.Core.Timing;

/// <summary>
///     Used to perform some common date-time operations.
/// </summary>
public static class Clock
{
    private static IClockProvider? _provider;

    /// <summary>
    ///     This object is used to perform all <see cref="Clock" /> operations.
    ///     Default value: <see cref="UnspecifiedClockProvider" />.
    /// </summary>
    public static IClockProvider Provider
    {
        get => _provider ?? ClockProviders.Unspecified;
        set => _provider =
            value ?? throw new ArgumentNullException(nameof(value), "Can not set Clock.Provider to null!");
    }

    /// <summary>
    ///     Gets Now using current <see cref="Provider" />.
    /// </summary>
    public static DateTime Now => Provider.Now;

    public static DateTimeKind Kind => Provider.Kind;

    /// <summary>
    ///     Returns true if multiple timezone is supported, returns false if not.
    /// </summary>
    public static bool SupportsMultipleTimezone => Provider.SupportsMultipleTimezone;

    /// <summary>
    ///     Normalizes given <see cref="DateTime" /> using current <see cref="Provider" />.
    /// </summary>
    /// <param name="dateTime">DateTime to be normalized.</param>
    /// <returns>Normalized DateTime</returns>
    public static DateTime Normalize(DateTime dateTime) => Provider.Normalize(dateTime);
}
