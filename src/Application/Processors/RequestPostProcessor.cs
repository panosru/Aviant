namespace Aviant.DDD.Application.Processors
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class RequestPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
        where TRequest : notnull
    {
    #region IRequestPostProcessor<TRequest,TResponse> Members

        public abstract Task Process(
            TRequest          request,
            TResponse         response,
            CancellationToken cancellationToken);

    #endregion
    }
}