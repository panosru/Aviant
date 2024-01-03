using Polly;

namespace Aviant.Core.Services;

public interface IRetry
{
    public IAsyncPolicy RetryPolicy();
}
