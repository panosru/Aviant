namespace Aviant.DDD.Application.Commands
{
    using Domain.Aggregates;
    using MediatR;

    public abstract class Command<TResponse> : ICommand<TResponse>
    { }

    public abstract class Command : Command<Unit>, ICommand
    { }

    public abstract class Command<TAggregateRoot, TAggregateId> : ICommand<TAggregateRoot, TAggregateId>
        where TAggregateRoot : class, IAggregateRoot<TAggregateId>
        where TAggregateId : class, IAggregateId
    { }
}