using System.Reflection;

namespace Aviant.Core.Reflection;

/// <summary>
///     Defines helper methods for reflection.
/// </summary>
internal static class ReflectionHelper
{
    /// <summary>
    ///     Checks whether <paramref name="givenType" /> implements/inherits <paramref name="genericType" />.
    /// </summary>
    /// <param name="givenType">Type to check</param>
    /// <param name="genericType">Generic type</param>
    public static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var givenTypeInfo = givenType.GetTypeInfo();

        if (givenTypeInfo.IsGenericType
         && givenType.GetGenericTypeDefinition() == genericType)
            return true;

        if (givenType.GetInterfaces()
           .ToList()
           .Exists(
                interfaceType => interfaceType.GetTypeInfo().IsGenericType
                              && interfaceType.GetGenericTypeDefinition() == genericType))
            return true;

        return givenTypeInfo.BaseType is not null && IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
    }

    /// <summary>
    ///     Gets a list of attributes defined for a class member and it's declaring type including inherited attributes.
    /// </summary>
    /// <param name="inherit">Inherit attribute from base classes</param>
    /// <param name="memberInfo">MemberInfo</param>
    public static List<object> GetAttributesOfMemberAndDeclaringType(MemberInfo memberInfo, bool inherit = true)
    {
        var attributeList = new List<object>();

        attributeList.AddRange(memberInfo.GetCustomAttributes(inherit));

        if (memberInfo.DeclaringType is not null)
            attributeList.AddRange(memberInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(inherit));

        return attributeList;
    }

    /// <summary>
    ///     Gets a list of attributes defined for a class member and it's declaring type including inherited attributes.
    /// </summary>
    /// <typeparam name="TAttribute">Type of the attribute</typeparam>
    /// <param name="memberInfo">MemberInfo</param>
    /// <param name="inherit">Inherit attribute from base classes</param>
    public static List<TAttribute> GetAttributesOfMemberAndDeclaringType<TAttribute>(
        MemberInfo memberInfo,
        bool       inherit = true)
        where TAttribute : Attribute
    {
        var attributeList = new List<TAttribute>();

        if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());

        if (memberInfo.DeclaringType?.GetTypeInfo().IsDefined(typeof(TAttribute), inherit) == true)
            attributeList.AddRange(
                memberInfo.DeclaringType.GetTypeInfo()
                   .GetCustomAttributes(typeof(TAttribute), inherit)
                   .Cast<TAttribute>());

        return attributeList;
    }

    /// <summary>
    ///     Gets a list of attributes defined for a class member and type including inherited attributes.
    /// </summary>
    /// <param name="memberInfo">MemberInfo</param>
    /// <param name="type">Type</param>
    /// <param name="inherit">Inherit attribute from base classes</param>
    public static List<object> GetAttributesOfMemberAndType(
        MemberInfo memberInfo,
        Type       type,
        bool       inherit = true)
    {
        var attributeList = new List<object>();
        attributeList.AddRange(memberInfo.GetCustomAttributes(inherit));
        attributeList.AddRange(type.GetTypeInfo().GetCustomAttributes(inherit));
        return attributeList;
    }

    /// <summary>
    ///     Gets a list of attributes defined for a class member and type including inherited attributes.
    /// </summary>
    /// <typeparam name="TAttribute">Type of the attribute</typeparam>
    /// <param name="memberInfo">MemberInfo</param>
    /// <param name="type">Type</param>
    /// <param name="inherit">Inherit attribute from base classes</param>
    public static List<TAttribute> GetAttributesOfMemberAndType<TAttribute>(
        MemberInfo memberInfo,
        Type       type,
        bool       inherit = true)
        where TAttribute : Attribute
    {
        var attributeList = new List<TAttribute>();

        if (memberInfo.IsDefined(typeof(TAttribute), inherit))
            attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());

        if (type.GetTypeInfo().IsDefined(typeof(TAttribute), inherit))
            attributeList.AddRange(
                type.GetTypeInfo().GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>());

        return attributeList;
    }

    /// <summary>
    ///     Tries to gets an of attribute defined for a class member and it's declaring type including inherited attributes.
    ///     Returns default value if it's not declared at all.
    /// </summary>
    /// <typeparam name="TAttribute">Type of the attribute</typeparam>
    /// <param name="memberInfo">MemberInfo</param>
    /// <param name="defaultValue">Default value (null as default)</param>
    public static TAttribute GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<TAttribute>(
        MemberInfo memberInfo,
        TAttribute defaultValue = default!)
        where TAttribute : class => memberInfo.GetCustomAttributes(true).OfType<TAttribute>().FirstOrDefault()
                                 ?? memberInfo.ReflectedType?.GetTypeInfo()
                                       .GetCustomAttributes(true)
                                       .OfType<TAttribute>()
                                       .FirstOrDefault()
                                 ?? defaultValue;

    /// <summary>
    ///     Tries to gets an of attribute defined for a class member and it's declaring type including inherited attributes.
    ///     Returns default value if it's not declared at all.
    /// </summary>
    /// <typeparam name="TAttribute">Type of the attribute</typeparam>
    /// <param name="memberInfo">MemberInfo</param>
    /// <param name="defaultValue">Default value (null as default)</param>
    /// <param name="inherit">Inherit attribute from base classes</param>
    public static TAttribute GetSingleAttributeOrDefault<TAttribute>(
        MemberInfo memberInfo,
        TAttribute defaultValue = default!,
        bool       inherit      = true)
        where TAttribute : Attribute =>
        //Get attribute on the member
        memberInfo.IsDefined(typeof(TAttribute), inherit)
            ? memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().First()
            : defaultValue;

    /// <summary>
    ///     Gets a property by it's full path from given object
    /// </summary>
    /// <param name="obj">Object to get value from</param>
    /// <param name="objectType">Type of given object</param>
    /// <param name="propertyPath">Full path of property</param>
    /// <param name="comparisonType">StringComparison type</param>
    /// <returns></returns>
    internal static object GetPropertyByPath(
        object           obj,
        Type             objectType,
        string           propertyPath,
        StringComparison comparisonType = StringComparison.Ordinal)
    {
        var property             = obj;
        var currentType          = objectType;
        var objectPath           = currentType.FullName;
        var absolutePropertyPath = propertyPath;

        if (objectPath is not null
         && absolutePropertyPath.StartsWith(objectPath, StringComparison.Ordinal))
            absolutePropertyPath = absolutePropertyPath.Replace(
                objectPath + ".",
                "",
                comparisonType
            );

        foreach (var propertyName in absolutePropertyPath.Split('.'))
        {
            property    = currentType.GetProperty(propertyName);
            currentType = ((PropertyInfo)property!).PropertyType;
        }

        return property;
    }

    /// <summary>
    ///     Gets value of a property by it's full path from given object
    /// </summary>
    /// <param name="obj">Object to get value from</param>
    /// <param name="objectType">Type of given object</param>
    /// <param name="propertyPath">Full path of property</param>
    /// <param name="comparisonType"></param>
    /// <returns></returns>
    internal static object? GetValueByPath(
        object           obj,
        Type             objectType,
        string           propertyPath,
        StringComparison comparisonType = StringComparison.Ordinal)
    {
        var value                = obj;
        var currentType          = objectType;
        var objectPath           = currentType.FullName;
        var absolutePropertyPath = propertyPath;

        if (objectPath is not null
         && absolutePropertyPath.StartsWith(objectPath, StringComparison.Ordinal))
            absolutePropertyPath = absolutePropertyPath.Replace(
                objectPath + ".",
                "",
                comparisonType
            );

        foreach (var property in absolutePropertyPath.Split('.')
                    .Select(propertyName => currentType.GetProperty(propertyName)))
        {
            value       = property?.GetValue(value, null);
            currentType = property?.PropertyType;
        }

        return value;
    }

    /// <summary>
    ///     Sets value of a property by it's full path on given object
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="objectType"></param>
    /// <param name="propertyPath"></param>
    /// <param name="value"></param>
    /// <param name="comparisonType"></param>
    internal static void SetValueByPath(
        object?          obj,
        Type             objectType,
        string           propertyPath,
        object           value,
        StringComparison comparisonType = StringComparison.Ordinal)
    {
        var           currentType = objectType;
        PropertyInfo? property;
        var           objectPath           = currentType.FullName;
        var           absolutePropertyPath = propertyPath;

        if (objectPath is not null
         && absolutePropertyPath.StartsWith(objectPath, StringComparison.Ordinal))
            absolutePropertyPath = absolutePropertyPath.Replace(
                objectPath + ".",
                "",
                comparisonType
            );

        var properties = absolutePropertyPath.Split('.');

        if (properties.Length == 1)
        {
            property = objectType.GetProperty(properties[0]);
            property?.SetValue(obj, value);
            return;
        }

        for (var i = 0; i < properties.Length - 1; i++)
        {
            property    = currentType?.GetProperty(properties[i]);
            obj         = property?.GetValue(obj, null);
            currentType = property?.PropertyType;
        }

        property = currentType?.GetProperty(properties[^1]);
        property?.SetValue(obj, value);
    }

    internal static bool IsPropertyGetterSetterMethod(MethodInfo method, Type type)
    {
        if (!method.IsSpecialName)
            return false;

        if (method.Name.Length < 5)
            return false;

        return type.GetProperty(
                method.Name[4..],
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
            is not null;
    }

    internal static async Task<object?> InvokeAsync(
        MethodInfo      method,
        object          obj,
        params object[] parameters)
    {
        var task = (Task)method.Invoke(obj, parameters)!;
        await task.ConfigureAwait(false);
        var resultProperty = task.GetType().GetProperty("Result");
        return resultProperty?.GetValue(task);
    }
}
