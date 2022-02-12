namespace Aviant.EventSourcing.Application.Commands;

using Core.Aggregates;
using MediatR;

public interface ICommand<out TAggregate, in TAggregateId> : IRequest<TAggregate>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{ }
