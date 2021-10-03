namespace Aviant.DDD.Core.Extensions
{
    using System;
    using System.Runtime.ExceptionServices;
    using System.Text.RegularExpressions;

    /// <summary>
    ///     Extension methods for <see cref="Exception" /> class.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        ///     Uses <see cref="Capture" /> method to re-throws exception
        ///     while preserving stack trace.
        /// </summary>
        /// <param name="exception">Exception to be re-thrown</param>
        public static void ReThrow(this Exception exception) => ExceptionDispatchInfo.Capture(exception).Throw();
    }
}
