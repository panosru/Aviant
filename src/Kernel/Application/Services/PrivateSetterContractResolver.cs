using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Aviant.Application.Services;

/// <inheritdoc />
/// <summary>
///     https://www.mking.net/blog/working-with-private-setters-in-json-net
/// </summary>
public sealed class PrivateSetterContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var jsonProperty = base.CreateProperty(member, memberSerialization);

        if (jsonProperty.Writable
         || member is not PropertyInfo propertyInfo)
            return jsonProperty;

        jsonProperty.Writable = propertyInfo.GetSetMethod(true) is not null;

        return jsonProperty;
    }
}
