namespace Aviant.DDD.Application.Commands
{
    #region

    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates;
    using Domain.Services;
    using MediatR;

    #endregion

    public abstract class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        #region ICommandHandler<TCommand,TResponse> Members

        public abstract Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);

        #endregion
    }

    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand<Unit>
    {
        #region ICommandHandler<TCommand> Members

        public abstract Task<Unit> Handle(TCommand command, CancellationToken cancellationToken);

        #endregion
    }

    public abstract class CommandHandler<TCommand, TAggregate, TAggregateId>
        : ICommandHandler<TCommand, TAggregate, TAggregateId>
        where TCommand : ICommand<TAggregate, TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        protected IEventsService<TAggregate, TAggregateId> EventsService => ServiceLocator.ServiceContainer
          ?.GetService<IEventsService<TAggregate, TAggregateId>>(
                typeof(IEventsService<TAggregate, TAggregateId>));

        #region ICommandHandler<TCommand,TAggregate,TAggregateId> Members

        public abstract Task<TAggregate> Handle(TCommand command, CancellationToken cancellationToken);

        #endregion
    }
}