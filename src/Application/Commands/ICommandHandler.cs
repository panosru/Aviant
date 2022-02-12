namespace Aviant.Foundation.Application.Commands;

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
