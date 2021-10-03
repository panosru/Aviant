namespace Aviant.DDD.Core.Services
{
    using Polly;

    public interface IRetry
    {
        public IAsyncPolicy RetryPolicy();
    }
}
