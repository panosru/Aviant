namespace Aviant.DDD.Application.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates;
    using Domain.Services;
    using MediatR;

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

    public abstract class CommandHandler<TCommand, TAggregateRoot, TAggregateId>
        : ICommandHandler<TCommand, TAggregateRoot, TAggregateId>
        where TCommand : ICommand<TAggregateRoot, TAggregateId>
        where TAggregateRoot : class, IAggregateRoot<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        protected IEventsService<TAggregateRoot, TAggregateId> EventsService => ServiceLocator.ServiceContainer
          ?.GetService<IEventsService<TAggregateRoot, TAggregateId>>(
                typeof(IEventsService<TAggregateRoot, TAggregateId>));

    #region ICommandHandler<TCommand,TAggregateRoot,TAggregateId> Members

        public abstract Task<TAggregateRoot> Handle(TCommand command, CancellationToken cancellationToken);

    #endregion
    }
}