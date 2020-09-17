namespace Aviant.DDD.Application.Queries
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        #region IQueryHandler<TQuery,TResponse> Members

        public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);

        #endregion
    }
}