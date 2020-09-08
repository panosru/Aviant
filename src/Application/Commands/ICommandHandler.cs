namespace Aviant.DDD.Application.Commands
{
    using Domain.Aggregates;
    using MediatR;

    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    { }

    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand<Unit>
    { }

    public interface ICommandHandler<in TCommand, TAggregateRoot, TAggregateId>
        : IRequestHandler<TCommand, TAggregateRoot>
        where TCommand : ICommand<TAggregateRoot, TAggregateId>
        where TAggregateRoot : class, IAggregateRoot<TAggregateId>
        where TAggregateId : class, IAggregateId
    { }
}