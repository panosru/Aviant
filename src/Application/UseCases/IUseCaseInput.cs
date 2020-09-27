namespace Aviant.DDD.Application.UseCases
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUseCaseInput
    {
        public Task ValidateAsync(CancellationToken cancellationToken = default);
    }
}