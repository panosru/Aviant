namespace Aviant.Foundation.Application.Queries;

using Core.Services;
using MediatR;

public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>,
      IRetry
    where TQuery : IQuery<TResponse>
{ }
