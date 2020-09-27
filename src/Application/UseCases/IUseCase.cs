namespace Aviant.DDD.Application.UseCases
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUseCase<in TUseCaseOutput>
        where TUseCaseOutput : class, IUseCaseOutput
    {
        public void SetOutput(TUseCaseOutput output);
    }

    public interface IUseCaseExecute
    {
        public Task Execute(CancellationToken cancellationToken = default);
    }

    public interface IUseCaseExecute<in TUseCaseInput>
        where TUseCaseInput : class, IUseCaseInput
    {
        public Task Execute(TUseCaseInput input, CancellationToken cancellationToken = default);
    }
}