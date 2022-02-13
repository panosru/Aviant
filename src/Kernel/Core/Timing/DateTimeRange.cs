namespace Aviant.Core.Timing;

/// <inheritdoc />
/// <summary>
///     A basic implementation of <see cref="T:Aviant.Core.Timing.IDateTimeRange" /> to store a date range.
/// </summary>
[Serializable]
public sealed class DateTimeRange : IDateTimeRange
{
    /// <summary>
    ///     Creates a new <see cref="DateTimeRange" /> object.
    /// </summary>
    public DateTimeRange()
    { }

    /// <summary>
    ///     Creates a new <see cref="DateTimeRange" /> object from given <paramref name="startTime" /> and
    ///     <paramref name="endTime" />.
    /// </summary>
    /// <param name="startTime">Start time of the datetime range</param>
    /// <param name="endTime">End time of the datetime range</param>
    public DateTimeRange(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime;
        EndTime   = endTime;
    }

    /// <summary>
    ///     Creates a new <see cref="DateTimeRange" /> object from the given <paramref name="startTime" /> and
    ///     <paramref name="timeSpan" />.
    /// </summary>
    /// <param name="startTime">Start time of the datetime range</param>
    /// <param name="timeSpan">The span of time to calculate the EndTime</param>
    public DateTimeRange(DateTime startTime, TimeSpan timeSpan)
    {
        StartTime = startTime;
        TimeSpan  = timeSpan;
    }

    /// <summary>
    ///     Creates a new <see cref="DateTimeRange" /> object from given <paramref name="dateTimeRange" /> object.
    /// </summary>
    /// <param name="dateTimeRange">IDateTimeRange object</param>
    public DateTimeRange(IDateTimeRange dateTimeRange)
    {
        StartTime = dateTimeRange.StartTime;
        EndTime   = dateTimeRange.EndTime;
    }

    private static DateTime Now => Clock.Now;

    /// <summary>
    ///     Gets a date range represents yesterday.
    /// </summary>
    public static DateTimeRange Yesterday
    {
        get
        {
            var now = Now;
            return new DateTimeRange(now.Date.AddDays(-1), now.Date.AddMilliseconds(-1));
        }
    }

    /// <summary>
    ///     Gets a date range represents today.
    /// </summary>
    public static DateTimeRange Today
    {
        get
        {
            var now = Now;
            return new DateTimeRange(now.Date, now.Date.AddDays(1).AddMilliseconds(-1));
        }
    }

    /// <summary>
    ///     Gets a date range represents tomorrow.
    /// </summary>
    public static DateTimeRange Tomorrow
    {
        get
        {
            var now = Now;
            return new DateTimeRange(now.Date.AddDays(1), now.Date.AddDays(2).AddMilliseconds(-1));
        }
    }

    /// <summary>
    ///     Gets a date range represents the last month.
    /// </summary>
    public static DateTimeRange LastMonth
    {
        get
        {
            var now       = Now;
            var startTime = now.Date.AddDays(-now.Day + 1).AddMonths(-1);
            var endTime   = startTime.AddMonths(1).AddMilliseconds(-1);
            return new DateTimeRange(startTime, endTime);
        }
    }

    /// <summary>
    ///     Gets a date range represents this month.
    /// </summary>
    public static DateTimeRange ThisMonth
    {
        get
        {
            var now       = Now;
            var startTime = now.Date.AddDays(-now.Day + 1);
            var endTime   = startTime.AddMonths(1).AddMilliseconds(-1);
            return new DateTimeRange(startTime, endTime);
        }
    }

    /// <summary>
    ///     Gets a date range represents the next month.
    /// </summary>
    public static DateTimeRange NextMonth
    {
        get
        {
            var now       = Now;
            var startTime = now.Date.AddDays(-now.Day + 1).AddMonths(1);
            var endTime   = startTime.AddMonths(1).AddMilliseconds(-1);
            return new DateTimeRange(startTime, endTime);
        }
    }


    /// <summary>
    ///     Gets a date range represents the last year.
    /// </summary>
    public static DateTimeRange LastYear
    {
        get
        {
            var now = Now;

            return new DateTimeRange(
                new DateTime(now.Year - 1, 1, 1),
                new DateTime(now.Year, 1, 1).AddMilliseconds(-1));
        }
    }

    /// <summary>
    ///     Gets a date range represents this year.
    /// </summary>
    public static DateTimeRange ThisYear
    {
        get
        {
            var now = Now;

            return new DateTimeRange(
                new DateTime(now.Year, 1, 1),
                new DateTime(now.Year + 1, 1, 1).AddMilliseconds(-1));
        }
    }

    /// <summary>
    ///     Gets a date range represents the next year.
    /// </summary>
    public static DateTimeRange NextYear
    {
        get
        {
            var now = Now;

            return new DateTimeRange(
                new DateTime(now.Year + 1, 1, 1),
                new DateTime(now.Year + 2, 1, 1).AddMilliseconds(-1));
        }
    }


    /// <summary>
    ///     Gets a date range represents the last 30 days (30x24 hours) including today.
    /// </summary>
    public static DateTimeRange Last30Days
    {
        get
        {
            var now = Now;
            return new DateTimeRange(now.AddDays(-30), now);
        }
    }

    /// <summary>
    ///     Gets a date range represents the last 30 days excluding today.
    /// </summary>
    public static DateTimeRange Last30DaysExceptToday
    {
        get
        {
            var now = Now;
            return new DateTimeRange(now.Date.AddDays(-30), now.Date.AddMilliseconds(-1));
        }
    }

    /// <summary>
    ///     Gets a date range represents the last 7 days (7x24 hours) including today.
    /// </summary>
    public static DateTimeRange Last7Days
    {
        get
        {
            var now = Now;
            return new DateTimeRange(now.AddDays(-7), now);
        }
    }

    /// <summary>
    ///     Gets a date range represents the last 7 days excluding today.
    /// </summary>
    public static DateTimeRange Last7DaysExceptToday
    {
        get
        {
            var now = Now;
            return new DateTimeRange(now.Date.AddDays(-7), now.Date.AddMilliseconds(-1));
        }
    }

    #region IDateTimeRange Members

    /// <inheritdoc />
    /// <summary>
    ///     Start time of the datetime range.
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <inheritdoc />
    /// <summary>
    ///     End time of the datetime range.
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <inheritdoc />
    /// <summary>
    ///     Gets the time span of the datetime range.
    ///     When set, EndTime is recalculated
    /// </summary>
    public TimeSpan TimeSpan
    {
        get => EndTime - StartTime;
        set => EndTime = StartTime.Add(value);
    }

    #endregion

    /// <summary>
    ///     Returns a <see cref="System.String" /> that represents the current <see cref="DateTimeRange" />.
    /// </summary>
    /// <returns>A <see cref="System.String" /> that represents the current <see cref="DateTimeRange" />.</returns>
    public override string ToString() => $"[{StartTime} - {EndTime}]";
}
