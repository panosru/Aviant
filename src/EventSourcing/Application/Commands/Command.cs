namespace Aviant.Application.EventSourcing.Commands;

using Core.EventSourcing.Aggregates;

public abstract record Command<TAggregate, TAggregateId> : ICommand<TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId;
