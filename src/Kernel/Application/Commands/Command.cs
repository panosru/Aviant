using MediatR;

namespace Aviant.Application.Commands;

public abstract record Command<TResponse> : ICommand<TResponse>;

public abstract record Command : Command<Unit>, ICommand;
