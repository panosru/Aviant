using System.Globalization;

namespace Aviant.Core.Enum;

/// <summary>
/// Enum extension methods.
/// </summary>
public static class EnumExtension
{
    /// <summary>
    /// Converts the value of this enum to its equivalent string representation in specified string case.
    /// </summary>
    /// <param name="enumValue">The enum value to convert.</param>
    /// <param name="stringCase">The case format to use.</param>
    /// <returns>The string representation of the enum value in the specified case.</returns>
    public static string ToString(this System.Enum enumValue, StringCase stringCase) => stringCase switch
    {
        StringCase.Upper => enumValue.ToString().ToUpper(CultureInfo.InvariantCulture),
        StringCase.Lower => enumValue.ToString().ToLower(CultureInfo.InvariantCulture),
        StringCase.Camel => ToCamelCase(enumValue.ToString()),
        StringCase.Capitalise => ToCapitaliseCase(enumValue.ToString()),
        _ => enumValue.ToString(),
    };

    /// <summary>
    /// Converts a string to camel case.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <returns>The camel case string.</returns>
    private static string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str, 0))
            return str;

        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }

    /// <summary>
    /// Converts a string to capitalise case.
    /// </summary>
    /// <param name="str">The string to convert.</param>
    /// <returns>The capitalise case string.</returns>
    private static string ToCapitaliseCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsUpper(str, 0))
            return str;

        return char.ToUpperInvariant(str[0]) + str.Substring(1);
    }
}
