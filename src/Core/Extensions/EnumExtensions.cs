namespace Aviant.Core.Extensions;

using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

// Credits: https://gist.github.com/cocowalla

public static class EnumExtensions
{
    // Note that we never need to expire these cache items, so we just use ConcurrentDictionary rather than MemoryCache
    private static readonly
        ConcurrentDictionary<string, string> DisplayNameCache = new();

    public static string DisplayName(this Enum value)
    {
        var key = $"{value.GetType().FullName}.{value}";

        var displayName = DisplayNameCache.GetOrAdd(
            key,
            _ =>
            {
                var name = (DescriptionAttribute[])value
                   .GetType()
                   .GetTypeInfo()
                   .GetField(value.ToString())!
                   .GetCustomAttributes(typeof(DescriptionAttribute), false);

                return name.Length > 0
                    ? name[0].Description
                    : value.ToString();
            });

        return displayName;
    }
}
