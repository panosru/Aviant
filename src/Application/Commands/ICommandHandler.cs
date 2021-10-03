namespace Aviant.DDD.Application.Commands
{
    using Core.Aggregates;
    using Core.Services;
    using MediatR;

    internal interface ICommandHandler<in TCommand, TResponse>
        : IRequestHandler<TCommand, TResponse>,
          IRetry
        where TCommand : ICommand<TResponse>
    { }

    internal interface ICommandHandler<in TCommand>
        : IRequestHandler<TCommand>,
          IRetry
        where TCommand : ICommand<Unit>
    { }

    internal interface ICommandHandler<in TCommand, TAggregate, TAggregateId>
        : IRequestHandler<TCommand, TAggregate>,
          IRetry
        where TCommand : ICommand<TAggregate, TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    { }
}
