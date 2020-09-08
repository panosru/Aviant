namespace Aviant.DDD.Application.Commands
{
    using Domain.Aggregates;
    using MediatR;

    public interface ICommand<out TAggregateRoot, in TAggregateId> : IRequest<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot<TAggregateId>
        where TAggregateId : class, IAggregateId
    { }

    public interface ICommand<out TResponse> : IRequest<TResponse>
    { }

    public interface ICommand : ICommand<Unit>
    { }
}