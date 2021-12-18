namespace Aviant.DDD.Application.Queries;

public abstract record Query<TResponse> : IQuery<TResponse>;
