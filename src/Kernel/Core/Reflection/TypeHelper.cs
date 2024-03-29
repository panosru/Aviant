using System.Reflection;

namespace Aviant.Core.Reflection;

/// <summary>
///     Some simple type-checking methods used internally.
/// </summary>
internal static class TypeHelper
{
    public static bool IsFunc(object? obj)
    {
        var type = obj?.GetType();

        if (type?.GetTypeInfo().IsGenericType != true)
            return false;

        return type.GetGenericTypeDefinition() == typeof(Func<>);
    }

    public static bool IsFunc<TReturn>(object? obj) => obj is Func<TReturn>;

    public static bool IsPrimitiveExtendedIncludingNullable(Type type, bool includeEnums = false)
    {
        if (IsPrimitiveExtended(type, includeEnums))
            return true;

        if (type.GetTypeInfo().IsGenericType
         && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            return IsPrimitiveExtended(type.GenericTypeArguments[0], includeEnums);

        return false;
    }

    private static bool IsPrimitiveExtended(Type type, bool includeEnums)
    {
        if (type.GetTypeInfo().IsPrimitive)
            return true;

        if (includeEnums && type.GetTypeInfo().IsEnum)
            return true;

        return type == typeof(string)
            || type == typeof(decimal)
            || type == typeof(DateTime)
            || type == typeof(DateTimeOffset)
            || type == typeof(TimeSpan)
            || type == typeof(Guid);
    }
}
