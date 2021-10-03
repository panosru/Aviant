namespace Aviant.DDD.Core.Enum
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    // Source taken from https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/enumeration-classes-over-enum-types#implement-an-enumeration-base-class
    public abstract class Enumeration
        : IEquatable<Enumeration>
    {
        public string Name { get; }

        public int Id { get; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<T> Getall<T>()
            where T : Enumeration => typeof(T).GetFields(
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
           .Select(f => f.GetValue(null))
           .Cast<T>();

        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration otherValue)
                return false;

            var typeMatches  = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        /// <inheritdoc />
        public bool Equals(Enumeration? other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Name == other.Name
                && Id   == other.Id;
        }

        /// <inheritdoc />
        public override int GetHashCode() => HashCode.Combine(Name, Id);

        public static bool operator ==(Enumeration? left, Enumeration? right) => Equals(left, right);

        public static bool operator !=(Enumeration? left, Enumeration? right) => !Equals(left, right);

        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}
