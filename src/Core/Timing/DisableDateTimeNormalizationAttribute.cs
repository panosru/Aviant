namespace Aviant.DDD.Core.Timing
{
    using System;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
    public sealed class DisableDateTimeNormalizationAttribute : Attribute
    { }
}
