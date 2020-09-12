namespace Aviant.DDD.Application.Queries
{
    #region

    using MediatR;

    #endregion

    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    { }
}