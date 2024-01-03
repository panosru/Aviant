using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Aviant.Core.Collections.Extensions;
using JetBrains.Annotations;

namespace Aviant.Core.Extensions;

/// <summary>
///     Extension methods for String class.
/// </summary>
public static partial class StringExtensions
{
    [GeneratedRegex("[a-z][A-Z]")]
    private static partial Regex GetAZiRegex();
    
    /// <summary>
    ///     Adds a char to end of given string if it does not ends with the char.
    /// </summary>
    public static string EnsureEndsWith(this string str, char c) =>
        EnsureEndsWith(str, c, StringComparison.Ordinal);

    /// <summary>
    ///     Adds a char to end of given string if it does not ends with the char.RemovePostFix
    /// </summary>
    public static string EnsureEndsWith(
        this string      str,
        char             c,
        StringComparison comparisonType)
    {
        if (str is null)
            throw new ArgumentNullException(nameof(str));

        if (str.EndsWith(c.ToString(CultureInfo.InvariantCulture), comparisonType))
            return str;

        return str + c;
    }

    /// <summary>
    ///     Adds a char to end of given string if it does not ends with the char.
    /// </summary>
    public static string EnsureEndsWith(
        this string str,
        char        c,
        bool        ignoreCase,
        CultureInfo culture)
    {
        if (str is null)
            throw new ArgumentNullException(nameof(str));

        if (str.EndsWith(c.ToString(culture), ignoreCase, culture))
            return str;

        return str + c;
    }

    /// <summary>
    ///     Adds a char to beginning of given string if it does not starts with the char.
    /// </summary>
    public static string EnsureStartsWith(this string str, char c) =>
        EnsureStartsWith(str, c, StringComparison.Ordinal);

    /// <summary>
    ///     Adds a char to beginning of given string if it does not starts with the char.
    /// </summary>
    public static string EnsureStartsWith(
        this string      str,
        char             c,
        StringComparison comparisonType)
    {
        if (str is null)
            throw new ArgumentNullException(nameof(str));

        if (str.StartsWith(c.ToString(CultureInfo.InvariantCulture), comparisonType))
            return str;

        return c + str;
    }

    /// <summary>
    ///     Adds a char to beginning of given string if it does not starts with the char.
    /// </summary>
    public static string EnsureStartsWith(
        this string str,
        char        c,
        bool        ignoreCase,
        CultureInfo culture)
    {
        if (str is null)
            throw new ArgumentNullException(nameof(str));

        if (str.StartsWith(c.ToString(culture), ignoreCase, culture))
            return str;

        return c + str;
    }

    /// <summary>
    ///     Indicates whether this string is null or an System.String.Empty string.
    /// </summary>
    [ContractAnnotation("null => true")]
    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

    /// <summary>
    ///     indicates whether this string is null, empty, or consists only of white-space characters.
    /// </summary>
    [ContractAnnotation("null => true")]
    public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);

    /// <summary>
    ///     Gets a substring of a string from beginning of the string.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="len" /> is bigger that string's length</exception>
    public static string Left(this string str, int len)
    {
        if (str is null)
            throw new ArgumentNullException(nameof(str));

        if (str.Length < len)
            throw new ArgumentException("len argument can not be bigger than given string's length!");

        return str[..len];
    }

    /// <summary>
    ///     Converts line endings in the string to <see cref="Environment.NewLine" />.
    /// </summary>
    public static string NormalizeLineEndings(this string str) =>
        str
           .Replace("\r\n", "\n",                StringComparison.Ordinal)
           .Replace("\r",   "\n",                StringComparison.Ordinal)
           .Replace("\n",   Environment.NewLine, StringComparison.Ordinal);

    /// <summary>
    ///     Converts line endings in the string to <see cref="Environment.NewLine" />.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="comparisonType">Comparison Type</param>
    /// <returns></returns>
    public static string NormalizeLineEndings(this string str, StringComparison comparisonType) =>
        str
           .Replace("\r\n", "\n",                comparisonType)
           .Replace("\r",   "\n",                comparisonType)
           .Replace("\n",   Environment.NewLine, comparisonType);

    /// <summary>
    ///     Gets index of nth occurence of a char in a string.
    /// </summary>
    /// <param name="str">source string to be searched</param>
    /// <param name="c">Char to search in <paramref name="str" /></param>
    /// <param name="n">Count of the occurence</param>
    public static int NthIndexOf(
        this string str,
        char        c,
        int         n)
    {
        if (str is null)
            throw new ArgumentNullException(nameof(str));

        var count = 0;

        for (var i = 0; i < str.Length; i++)
        {
            if (str[i] != c)
                continue;

            if (++count == n)
                return i;
        }

        return -1;
    }

    /// <summary>
    ///     Removes first occurrence of the given postfixes from end of the given string.
    ///     Ordering is important. If one of the postFixes is matched, others will not be tested.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="postFixes">one or more postfix.</param>
    /// <returns>Modified string or the same string if it has not any of given postfixes</returns>
    public static string? RemovePostFix(this string? str, params string[] postFixes)
    {
        if (str is null)
            return null;

        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return postFixes.IsNullOrEmpty()
            ? str
            : str.Left(str.Length - (postFixes.ToList().Find(str.EndsWith)?.Length ?? 0));
    }

    /// <summary>
    ///     Removes first occurrence of the given prefixes from beginning of the given string.
    ///     Ordering is important. If one of the preFixes is matched, others will not be tested.
    /// </summary>
    /// <param name="str">The string.</param>
    /// <param name="preFixes">one or more prefix.</param>
    /// <returns>Modified string or the same string if it has not any of given prefixes</returns>
    public static string? RemovePreFix(this string? str, params string[] preFixes)
    {
        if (str is null)
            return null;

        if (string.IsNullOrEmpty(str))
            return string.Empty;

        return preFixes.IsNullOrEmpty()
            ? str
            : str.Right(str.Length - (preFixes.ToList().Find(str.StartsWith)?.Length ?? 0));
    }

    /// <summary>
    ///     Gets a substring of a string from end of the string.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="len" /> is bigger that string's length</exception>
    public static string Right(this string str, int len)
    {
        if (str is null)
            throw new ArgumentNullException(nameof(str));

        if (str.Length < len)
            throw new ArgumentException("len argument can not be bigger than given string's length!");

        return str.Substring(str.Length - len, len);
    }

    /// <summary>
    ///     Uses string.Split method to split given string by given separator.
    /// </summary>
    public static string[] Split(this string str, string separator)
    {
        return str.Split(new[] { separator }, StringSplitOptions.None);
    }

    /// <summary>
    ///     Uses string.Split method to split given string by given separator.
    /// </summary>
    public static string[] Split(
        this string        str,
        string             separator,
        StringSplitOptions options)
    {
        return str.Split(new[] { separator }, options);
    }

    /// <summary>
    ///     Uses string.Split method to split given string by <see cref="Environment.NewLine" />.
    /// </summary>
    public static string[] SplitToLines(this string str) => str.Split(Environment.NewLine);

    /// <summary>
    ///     Uses string.Split method to split given string by <see cref="Environment.NewLine" />.
    /// </summary>
    public static string[] SplitToLines(this string str, StringSplitOptions options) =>
        str.Split(Environment.NewLine, options);

    /// <summary>
    ///     Converts PascalCase string to camelCase.
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <returns>camelCase of the string</returns>
    public static string ToCamelCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        if (str.Length == 1)
            return str.ToLowerInvariant();

        return char.ToLowerInvariant(str[0]) + str[1..];
    }

    /// <summary>
    ///     Converts given PascalCase/camelCase string to sentence (by splitting words by space).
    ///     Example: "ThisIsSampleSentence" is converted to "This is a sample sentence".
    /// </summary>
    /// <param name="str">String to convert.</param>
    public static string ToSentenceCase(this string str)
    {
        return string.IsNullOrWhiteSpace(str)
            ? str
            : GetAZiRegex().Replace(
                str,
                m =>
                    m.Value[0] + " " + char.ToLowerInvariant(m.Value[1]));
    }

    /// <summary>
    ///     Converts string to enum value.
    /// </summary>
    /// <typeparam name="T">Type of enum</typeparam>
    /// <param name="value">String value to convert</param>
    /// <returns>Returns enum object</returns>
    public static T ToEnum<T>(this string value)
        where T : struct
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        return (T)System.Enum.Parse(typeof(T), value);
    }

    /// <summary>
    ///     Converts string to enum value.
    /// </summary>
    /// <typeparam name="T">Type of enum</typeparam>
    /// <param name="value">String value to convert</param>
    /// <param name="ignoreCase">Ignore case</param>
    /// <returns>Returns enum object</returns>
    public static T ToEnum<T>(this string value, bool ignoreCase)
        where T : struct
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        return (T)System.Enum.Parse(typeof(T), value, ignoreCase);
    }

    public static string ToMd5(this string str)
    {
        using var md5 = MD5.Create();

        var inputBytes = Encoding.UTF8.GetBytes(str);
        var hashBytes = MD5.HashData(inputBytes);

        var sb = new StringBuilder();

        foreach (var hashByte in hashBytes)
            sb.Append(hashByte.ToString("X2", CultureInfo.InvariantCulture));

        return sb.ToString();
    }

    /// <summary>
    ///     Converts camelCase string to PascalCase.
    /// </summary>
    /// <param name="str">String to convert</param>
    /// <returns>PascalCase of the string</returns>
    public static string ToPascalCase(this string str)
    {
        if (string.IsNullOrWhiteSpace(str))
            return str;

        if (str.Length == 1)
            return str.ToUpperInvariant();

        return char.ToUpperInvariant(str[0]) + str[1..];
    }

    /// <summary>
    ///     Gets a substring of a string from beginning of the string if it exceeds maximum length.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
    public static string? Truncate(this string? str, int maxLength)
    {
        if (str is null)
            return null;

        return str.Length <= maxLength
            ? str
            : str.Left(maxLength);
    }

    /// <summary>
    ///     Gets a substring of a string from beginning of the string if it exceeds maximum length.
    ///     It adds a "..." postfix to end of the string if it's truncated.
    ///     Returning string can not be longer than maxLength.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
    public static string? TruncateWithPostfix(this string? str, int maxLength) =>
        TruncateWithPostfix(str, maxLength, "...");

    /// <summary>
    ///     Gets a substring of a string from beginning of the string if it exceeds maximum length.
    ///     It adds given <paramref name="postfix" /> to end of the string if it's truncated.
    ///     Returning string can not be longer than maxLength.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str" /> is null</exception>
    public static string? TruncateWithPostfix(
        this string? str,
        int          maxLength,
        string       postfix)
    {
        if (str is null)
            return null;

        if (string.IsNullOrEmpty(str)
         || maxLength == 0)
            return string.Empty;

        if (str.Length <= maxLength)
            return str;

        if (maxLength <= postfix.Length)
            return postfix.Left(maxLength);

        return str.Left(maxLength - postfix.Length) + postfix;
    }
}
