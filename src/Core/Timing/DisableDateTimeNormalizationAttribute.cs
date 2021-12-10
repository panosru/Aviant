namespace Aviant.DDD.Core.Timing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class DisableDateTimeNormalizationAttribute : Attribute
{ }
