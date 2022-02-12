namespace Aviant.Foundation.Core.ValueObjects;

internal interface IValueObject<T>
    : IEquatable<T>
{
    public int GetHashCode();

    public int HashValue(int seed, object value);
}
