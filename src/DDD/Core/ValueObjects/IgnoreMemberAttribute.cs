namespace Aviant.Core.DDD.ValueObjects;

// source: https://github.com/jhewlett/ValueObject
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
internal sealed class IgnoreMemberAttribute : Attribute;
