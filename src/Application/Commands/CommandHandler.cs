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
        public abstract Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);
    }
    
    public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand<Unit>
    {
        public abstract Task<Unit> Handle(TCommand command, CancellationToken cancellationToken);
    }

    public abstract class CommandHandler<TCommand, TAggregateRoot, TKey>
        : ICommandHandler<TCommand, TAggregateRoot, TKey>
            where TCommand : ICommand<TAggregateRoot, TKey>
            where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        protected IEventsService<TAggregateRoot, TKey> EventsService =>
            ServiceLocator.ServiceContainer?.GetService<IEventsService<TAggregateRoot, TKey>>(
                typeof(IEventsService<TAggregateRoot, TKey>));

        public abstract Task<TAggregateRoot> Handle(TCommand command, CancellationToken cancellationToken);
    }
}