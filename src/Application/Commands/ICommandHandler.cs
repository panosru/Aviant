namespace Aviant.DDD.Application.Commands
{
    using Domain.Aggregates;
    using MediatR;

    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {}

    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand<Unit>
    {}
    
    public interface ICommandHandler<in TCommand, TAggregateRoot, TKey> 
        : IRequestHandler<TCommand, TAggregateRoot>
        where TCommand : ICommand<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {}
}