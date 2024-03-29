using System.Reflection;

namespace Aviant.Core.Reflection.Extensions;

/// <summary>
///     Extensions to <see cref="MemberInfo" />.
/// </summary>
public static class MemberInfoExtensions
{
    /// <summary>
    ///     Gets a single attribute for a member.
    /// </summary>
    /// <typeparam name="TAttribute">Type of the attribute</typeparam>
    /// <param name="memberInfo">The member that will be checked for the attribute</param>
    /// <param name="inherit">Include inherited attributes</param>
    /// <returns>Returns the attribute object if found. Returns null if not found.</returns>
    public static TAttribute? GetSingleAttributeOrNull<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
        where TAttribute : Attribute
    {
        if (memberInfo is null)
            throw new ArgumentNullException(nameof(memberInfo));

        var attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToArray();

        if (attrs.Length > 0)
            return (TAttribute)attrs[0];

        return default(TAttribute);
    }


    public static TAttribute? GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(this Type? type)
        where TAttribute : Attribute
    {
        while (true)
        {
            var attr = type?.GetTypeInfo().GetSingleAttributeOrNull<TAttribute>();

            if (attr is not null)
                return attr;

            if (type?.GetTypeInfo().BaseType is null)
                return null;

            type = type.GetTypeInfo().BaseType;
        }
    }
}
