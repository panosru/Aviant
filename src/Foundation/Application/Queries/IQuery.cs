namespace Aviant.Foundation.Application.Queries;

using MediatR;

public interface IQuery<out TResponse> : IRequest<TResponse>
{ }
