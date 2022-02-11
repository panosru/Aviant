namespace Aviant.Core.Services;

using Polly;

public interface IRetry
{
    public IAsyncPolicy RetryPolicy();
}
