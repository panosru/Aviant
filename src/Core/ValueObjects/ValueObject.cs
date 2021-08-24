namespace Aviant.DDD.Core.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    // Learn more: https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/microservice-ddd-cqrs-patterns/implement-value-objects
    // source: https://github.com/jhewlett/ValueObject
    public abstract class ValueObject : IValueObject<ValueObject>
    {
        private List<FieldInfo>? _fields;

        private List<PropertyInfo>? _properties;

        #region IValueObject<ValueObject> Members

        public override int GetHashCode()
        {
            var hash = GetProperties()
               .Select(property => property.GetValue(this, null))
               .Aggregate(17, HashValue);

            return GetFields()
               .Select(field => field.GetValue(this))
               .Aggregate(hash, HashValue);
        }

        public int HashValue(int seed, object? value)
        {
            var currentHash = value?.GetHashCode() ?? 0;

            return seed * 23 + currentHash;
        }

        public virtual bool Equals(ValueObject? other) => Equals(other as object);

        #endregion

        public static bool operator ==(ValueObject? left, ValueObject? right)
        {
            if (left is null ^ right is null) return false;

            return left?.Equals(right) != false;
        }

        public static bool operator !=(ValueObject? left, ValueObject? right) => !(left == right);

        public override bool Equals(object? obj)
        {
            if (obj is null
             || obj.GetType() != GetType()) return false;

            return GetProperties().All(p => PropertiesAreEqual(obj, p))
                && GetFields().All(f => FieldsAreEqual(obj, f));
        }

        private bool PropertiesAreEqual(object obj, PropertyInfo p) =>
            Equals(p.GetValue(this, null), p.GetValue(obj, null));

        private bool FieldsAreEqual(object obj, FieldInfo f) => Equals(f.GetValue(this), f.GetValue(obj));

        private IEnumerable<PropertyInfo> GetProperties()
        {
            return _properties ??= GetType()
               .GetProperties(BindingFlags.Instance | BindingFlags.Public)
               .Where(p => p.GetCustomAttribute(typeof(IgnoreMemberAttribute)) is null)
               .ToList();
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            return _fields ??= GetType()
               .GetFields(BindingFlags.Instance | BindingFlags.Public)
               .Where(f => f.GetCustomAttribute(typeof(IgnoreMemberAttribute)) is null)
               .ToList();
        }
    }
}