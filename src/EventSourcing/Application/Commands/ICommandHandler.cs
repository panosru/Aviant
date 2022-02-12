namespace Aviant.EventSourcing.Application.Commands;

using Aviant.Core.Services;
using Core.Aggregates;
using MediatR;

internal interface ICommandHandler<in TCommand, TAggregate, TAggregateId>
    : IRequestHandler<TCommand, TAggregate>,
      IRetry
    where TCommand : ICommand<TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{ }
