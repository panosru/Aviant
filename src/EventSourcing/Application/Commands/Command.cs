using Aviant.Core.EventSourcing.Aggregates;

namespace Aviant.Application.EventSourcing.Commands;

public abstract record Command<TAggregate, TAggregateId> : ICommand<TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId;
