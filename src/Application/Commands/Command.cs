namespace Aviant.DDD.Application.Commands
{
    using Domain.Aggregates;
    using MediatR;

    public abstract class Command<TResponse> : ICommand<TResponse>
    {}

    public abstract class Command : Command<Unit>, ICommand
    {}
    
    public abstract class Command<TAggregateRoot, TKey> : ICommand<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {}
}