namespace Aviant.EventSourcing.Core.Services;

using Aggregates;
using DomainEvents;

public interface IEventSerializer
{
    public IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string type, byte[] data)
        where TAggregateId : IAggregateId;

    public IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string type, string data)
        where TAggregateId : IAggregateId;

    public byte[] Serialize<TAggregateId>(IDomainEvent<TAggregateId> @event)
        where TAggregateId : IAggregateId;
}
