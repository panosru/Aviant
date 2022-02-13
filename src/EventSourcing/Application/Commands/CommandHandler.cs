namespace Aviant.Application.EventSourcing.Commands;

using Aviant.Core.Services;
using Core.EventSourcing.Aggregates;
using Core.EventSourcing.Services;
using Polly;

public abstract class CommandHandler<TCommand, TAggregate, TAggregateId>
    : ICommandHandler<TCommand, TAggregate, TAggregateId>
    where TCommand : ICommand<TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    protected IEventsService<TAggregate, TAggregateId> EventsService =>
        ServiceLocator.ServiceContainer.GetRequiredService<IEventsService<TAggregate, TAggregateId>>(
            typeof(IEventsService<TAggregate, TAggregateId>));

    #region ICommandHandler<TCommand,TAggregate,TAggregateId> Members

    public abstract Task<TAggregate> Handle(TCommand command, CancellationToken cancellationToken);

    public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

    #endregion
}
