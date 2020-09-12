namespace Aviant.DDD.Application.Commands
{
    #region

    using Domain.Aggregates;
    using MediatR;

    #endregion

    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    { }

    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
        where TCommand : ICommand<Unit>
    { }

    public interface ICommandHandler<in TCommand, TAggregate, TAggregateId>
        : IRequestHandler<TCommand, TAggregate>
        where TCommand : ICommand<TAggregate, TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    { }
}