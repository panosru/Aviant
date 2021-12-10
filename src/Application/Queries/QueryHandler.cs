namespace Aviant.DDD.Application.Queries;

using Polly;

public abstract class QueryHandler<TQuery, TResponse> : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    #region IQueryHandler<TQuery,TResponse> Members

    public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);

    public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

    #endregion
}
