namespace Aviant.Application.Commands;

using Core.Aggregates;
using MediatR;

public abstract record Command<TResponse> : ICommand<TResponse>;

public abstract record Command : Command<Unit>, ICommand;

public abstract record Command<TAggregate, TAggregateId> : ICommand<TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId;
