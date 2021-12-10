namespace Aviant.DDD.Application.Services;

using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Core.Aggregates;
using Core.DomainEvents;
using Core.Services;
using Newtonsoft.Json;

public sealed class JsonEventDeserializer
    : IEventDeserializer,
      IJsonEventDeserializerCache
{
    private readonly IEnumerable<Assembly> _assemblies;

    private readonly IJsonEventDeserializerCache _cache;

    public JsonEventDeserializer(IEnumerable<Assembly>? assemblies)
    {
        _cache = this;

        _assemblies = assemblies ?? new[] { Assembly.GetExecutingAssembly() };
    }

    #region IEventDeserializer Members

    public IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string type, byte[] data)
        where TAggregateId : IAggregateId
    {
        var jsonData = Encoding.UTF8.GetString(data);

        return Deserialize<TAggregateId>(type, jsonData);
    }

    public IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string type, string data)
        where TAggregateId : IAggregateId
    {
        var eventType = _cache.Exists(type)
            ? _cache.Get(type)
            : _assemblies
                 .Select(a => a.GetType(type, false))
                 .FirstOrDefault(t => t is not null)
           ?? Type.GetType(type);

        if (eventType is null)
            throw new ArgumentOutOfRangeException(nameof(type), $"invalid event type: {type}");

        if (!_cache.Exists(type))
            _cache.Add(type, eventType);

        // as of 01/10/2020, "Deserialization to reference types
        // without a parameterless constructor isn't supported."
        // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
        // apparently it's being worked on: https://github.com/dotnet/runtime/issues/29895

        var result = JsonConvert.DeserializeObject(
            data,
            eventType,
            new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ContractResolver    = new PrivateSetterContractResolver()
            });

        if (result is null)
            throw new SerializationException($"unable to deserialize event {type} : {data}");

        return (IDomainEvent<TAggregateId>)result;
    }

    #endregion
}
