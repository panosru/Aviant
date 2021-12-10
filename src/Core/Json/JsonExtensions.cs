namespace Aviant.DDD.Core.Json;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

public static class JsonExtensions
{
    /// <summary>
    ///     Converts given object to JSON string.
    /// </summary>
    /// <returns></returns>
    public static string ToJsonString(
        this object obj,
        bool        camelCase = false,
        bool        indented  = false)
    {
        var options = new JsonSerializerSettings();

        if (camelCase)
            options.ContractResolver = new CamelCasePropertyNamesContractResolver();

        if (indented)
            options.Formatting = Formatting.Indented;

        options.Converters.Insert(0, new DateTimeConverter());

        return JsonConvert.SerializeObject(obj, options);
    }

    /// <summary>
    ///     Converts given object to JSON string using custom <see cref="JsonSerializerSettings" />.
    /// </summary>
    /// <returns></returns>
    public static string ToJsonString(this object obj, JsonSerializerSettings settings) =>
        JsonConvert.SerializeObject(obj, settings);

    /// <summary>
    ///     Returns deserialized string using default <see cref="JsonSerializerSettings" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static T? FromJsonString<T>(this string value) => value.FromJsonString<T>(new JsonSerializerSettings());

    /// <summary>
    ///     Returns deserialized string using custom <see cref="JsonSerializerSettings" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static T? FromJsonString<T>(this string value, JsonSerializerSettings? settings) =>
        JsonConvert.DeserializeObject<T>(value, settings);

    /// <summary>
    ///     Returns deserialized string using explicit <see cref="Type" /> and custom <see cref="JsonSerializerSettings" />
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static object? FromJsonString(
        this string            value,
        Type                   type,
        JsonSerializerSettings settings)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        return JsonConvert.DeserializeObject(value, type, settings);
    }
}
