using Aviant.Core.Services;
using MediatR;

namespace Aviant.Application.Queries;

public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, TResponse>,
        IRetry
    where TQuery : IQuery<TResponse>;
