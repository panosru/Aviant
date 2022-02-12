namespace Aviant.Foundation.Core.Json;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Timing;

public sealed class DateTimeConverter : IsoDateTimeConverter
{
    public override bool CanConvert(Type objectType) =>
        objectType == typeof(DateTime) || objectType == typeof(DateTime?);

    public override object? ReadJson(
        JsonReader     reader,
        Type           objectType,
        object?        existingValue,
        JsonSerializer serializer)
    {
        if (base.ReadJson(
                reader,
                objectType,
                existingValue,
                serializer) is DateTime date)
            return Clock.Normalize(date);

        return null;
    }

    public override void WriteJson(
        JsonWriter     writer,
        object?        value,
        JsonSerializer serializer)
    {
        var date = value as DateTime?;

        base.WriteJson(
            writer,
            date.HasValue
                ? Clock.Normalize(date.Value)
                : value,
            serializer);
    }
}
