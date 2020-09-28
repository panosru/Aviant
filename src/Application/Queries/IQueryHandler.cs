namespace Aviant.DDD.Application.Queries
{
    using MediatR;

    internal interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    { }
}