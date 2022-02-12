namespace Aviant.EventSourcing.Application.Commands;

using Core.Aggregates;

public abstract record Command<TAggregate, TAggregateId> : ICommand<TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId;
