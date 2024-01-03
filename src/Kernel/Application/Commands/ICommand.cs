using MediatR;

namespace Aviant.Application.Commands;

public interface ICommand<out TResponse> : IRequest<TResponse>;

public interface ICommand : ICommand<Unit>;
