namespace Aviant.DDD.Application.Queries
{
    using MediatR;

    public interface IQuery<out TResponse> : IRequest<TResponse>
    { }
}
