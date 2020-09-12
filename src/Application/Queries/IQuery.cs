namespace Aviant.DDD.Application.Queries
{
    #region

    using MediatR;

    #endregion

    public interface IQuery<out TResponse> : IRequest<TResponse>
    { }
}