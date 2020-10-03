namespace Aviant.DDD.Core.ValueObjects
{
    using System;

    internal interface IValueObject<T>
        : IEquatable<T>
    {
        public int GetHashCode();

        public int HashValue(int seed, object value);
    }
}