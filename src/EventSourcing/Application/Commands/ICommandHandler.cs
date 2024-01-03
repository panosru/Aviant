using Aviant.Core.EventSourcing.Aggregates;
using Aviant.Core.Services;
using MediatR;

namespace Aviant.Application.EventSourcing.Commands;

internal interface ICommandHandler<in TCommand, TAggregate, TAggregateId>
    : IRequestHandler<TCommand, TAggregate>,
      IRetry
    where TCommand : ICommand<TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{ }
