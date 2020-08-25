namespace Aviant.DDD.Domain.Services
{
    using Aggregates;
    using Events;

    public interface IEventDeserializer
    {
        IEvent<TKey> Deserialize<TKey>(string type, byte[] data);

        IEvent<TKey> Deserialize<TKey>(string type, string data);
    }
}