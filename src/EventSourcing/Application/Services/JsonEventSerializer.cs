namespace Aviant.EventSourcing.Application.Services;

using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Aviant.Application.Services;
using Core.Aggregates;
using Core.DomainEvents;
using Core.Services;
using Newtonsoft.Json;

public sealed class JsonEventSerializer
    : IEventSerializer
{
    private readonly IEnumerable<Assembly> _assemblies;

    private readonly ConcurrentDictionary<string, Type?> _cache = new();

    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        ContractResolver    = new PrivateSetterContractResolver()
    };

    public JsonEventSerializer(IEnumerable<Assembly>? assemblies)
    {
        _assemblies = assemblies ?? new[] { Assembly.GetExecutingAssembly() };
    }

    #region IEventSerializer Members

    public IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string type, byte[] data)
        where TAggregateId : IAggregateId
    {
        var jsonData = Encoding.UTF8.GetString(data);

        return Deserialize<TAggregateId>(type, jsonData);
    }

    public IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string type, string data)
        where TAggregateId : IAggregateId
    {
        var eventType = _cache.GetOrAdd(
            type,
            _ => _assemblies.Select(
                         a => a.GetType(type, false))
                    .FirstOrDefault(
                         t => t is not null)
              ?? Type.GetType(type));

        if (eventType is null)
            throw new ArgumentOutOfRangeException(nameof(type), $"invalid event type: {type}");

        // as of 01/10/2020, "Deserialization to reference types
        // without a parameterless constructor isn't supported."
        // https://docs.microsoft.com/en-us/dotnet/standard/serialization/system-text-json-how-to
        // apparently it's being worked on: https://github.com/dotnet/runtime/issues/29895

        var result = JsonConvert.DeserializeObject(data, eventType, JsonSerializerSettings);

        if (result is null)
            throw new SerializationException($"unable to deserialize event {type} : {data}");

        return (IDomainEvent<TAggregateId>)result;
    }

    public byte[] Serialize<TAggregateId>(IDomainEvent<TAggregateId> @event)
        where TAggregateId : IAggregateId =>
        Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize((dynamic)@event));

    #endregion
}
