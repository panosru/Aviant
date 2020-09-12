namespace Aviant.DDD.Application.Processors
{
    #region

    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public abstract class RequestPreProcessor<TRequest> : IRequestPreProcessor<TRequest>
        where TRequest : notnull
    {
        #region IRequestPreProcessor<TRequest> Members

        public abstract Task Process(TRequest request, CancellationToken cancellationToken);

        #endregion
    }
}