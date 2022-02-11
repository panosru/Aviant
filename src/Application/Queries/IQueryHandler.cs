namespace Aviant.Application.Queries;

using Core.Services;
using MediatR;

internal interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>,
      IRetry
    where TQuery : IQuery<TResponse>
{ }
