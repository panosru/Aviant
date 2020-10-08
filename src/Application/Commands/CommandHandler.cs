namespace Aviant.DDD.Application.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Aggregates;
    using Core.Services;
    using MediatR;

    public abstract class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public abstract Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }

    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand<Unit>
    {
        public abstract Task<Unit> Handle(TCommand command, CancellationToken cancellationToken);
    }

    public abstract class CommandHandler<TCommand, TAggregate, TAggregateId>
        : ICommandHandler<TCommand, TAggregate, TAggregateId>
        where TCommand : ICommand<TAggregate, TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        protected IEventsService<TAggregate, TAggregateId> EventsService =>
            ServiceLocator.ServiceContainer.GetRequiredService<IEventsService<TAggregate, TAggregateId>>(
                typeof(IEventsService<TAggregate, TAggregateId>));

        public abstract Task<TAggregate> Handle(TCommand command, CancellationToken cancellationToken);
    }
}