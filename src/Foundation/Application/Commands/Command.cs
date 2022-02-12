namespace Aviant.Foundation.Application.Commands;

using MediatR;

public abstract record Command<TResponse> : ICommand<TResponse>;

public abstract record Command : Command<Unit>, ICommand;
