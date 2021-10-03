namespace Aviant.DDD.Application.Commands
{
    using Core.Aggregates;
    using MediatR;

    public abstract class Command<TResponse> : ICommand<TResponse>
    { }

    public abstract class Command : Command<Unit>, ICommand
    { }

    public abstract class Command<TAggregate, TAggregateId> : ICommand<TAggregate, TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    { }
}
