using Aviant.Core.EventSourcing.Aggregates;
using MediatR;

namespace Aviant.Application.EventSourcing.Commands;

public interface ICommand<out TAggregate, in TAggregateId> : IRequest<TAggregate>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId;
