namespace Aviant.Application.Queries;

public abstract record Query<TResponse> : IQuery<TResponse>;
