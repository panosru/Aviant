using Aviant.Core.EventSourcing.Aggregates;
using Aviant.Core.EventSourcing.DomainEvents;

namespace Aviant.Core.EventSourcing.Services;

public interface IEventSerializer
{
    public IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string type, byte[] data)
        where TAggregateId : IAggregateId;

    public IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string type, string data)
        where TAggregateId : IAggregateId;

    public byte[] Serialize<TAggregateId>(IDomainEvent<TAggregateId> @event)
        where TAggregateId : IAggregateId;
}
