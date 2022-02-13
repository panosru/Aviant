namespace Aviant.Core.Timing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
public sealed class DisableDateTimeNormalizationAttribute : Attribute
{ }
