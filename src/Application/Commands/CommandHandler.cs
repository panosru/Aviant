namespace Aviant.Foundation.Application.Commands;

using MediatR;
using Polly;

public abstract class CommandHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand<TResponse>
{
    #region ICommandHandler<TCommand,TResponse> Members

    public abstract Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken);

    public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

    #endregion
}

public abstract class CommandHandler<TCommand> : ICommandHandler<TCommand>
    where TCommand : ICommand<Unit>
{
    #region ICommandHandler<TCommand> Members

    public abstract Task<Unit> Handle(TCommand command, CancellationToken cancellationToken);

    public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

    #endregion
}
