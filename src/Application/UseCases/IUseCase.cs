namespace Aviant.Application.UseCases;

public interface IUseCase<in TUseCaseOutput>
    where TUseCaseOutput : class, IUseCaseOutput
{
    public void SetOutput(TUseCaseOutput output);
}

internal interface IUseCaseExecute
{
    public Task ExecuteAsync(CancellationToken cancellationToken = default);
}

internal interface IUseCaseExecute<in TUseCaseInput>
    where TUseCaseInput : class, IUseCaseInput
{
    public Task ExecuteAsync(TUseCaseInput input, CancellationToken cancellationToken = default);
}
