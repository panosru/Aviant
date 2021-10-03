namespace Aviant.DDD.Core.Timing
{
    using System;

    /// <inheritdoc />
    /// <summary>
    /// Implements <see cref="T:Aviant.DDD.Core.Timing.IClockProvider" /> to work with UTC times.
    /// </summary>
    public sealed class UtcClockProvider : IClockProvider
    {
        public DateTime Now => DateTime.UtcNow;

        public DateTimeKind Kind => DateTimeKind.Utc;

        public bool SupportsMultipleTimezone => true;

        public DateTime Normalize(DateTime dateTime)
        {
            return dateTime.Kind switch
            {
                DateTimeKind.Unspecified => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc),
                DateTimeKind.Local       => dateTime.ToUniversalTime(),
                _                        => dateTime
            };
        }

        internal UtcClockProvider()
        { }
    }
}
