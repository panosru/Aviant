using MediatR;

namespace Aviant.Application.Queries;

public interface IQuery<out TResponse> : IRequest<TResponse>;
