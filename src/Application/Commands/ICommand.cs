namespace Aviant.DDD.Application.Commands
{
    using Domain.Aggregates;
    using MediatR;

    public interface ICommand<out TAggregateRoot, in TKey> : IRequest<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {}
    
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {}

    public interface ICommand : ICommand<Unit>
    {}
}