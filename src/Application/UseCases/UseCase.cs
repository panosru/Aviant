// ReSharper disable MemberCanBePrivate.Global

#pragma warning disable 8618
namespace Aviant.DDD.Application.UseCases
{
    using System.Threading.Tasks;
    using Core.Services;
    using Orchestration;

    public abstract class UseCase<TUseCaseOutput> : IUseCase
        where TUseCaseOutput : class, IUseCaseOutput
    {
        protected TUseCaseOutput Output;

        protected IOrchestrator Orchestrator =>
            ServiceLocator.ServiceContainer.GetRequiredService<IOrchestrator>(
                typeof(IOrchestrator));

        public Task ExecuteAsync(TUseCaseOutput output)
        {
            SetOutput(output);

            return Execute();
        }

        protected abstract Task Execute();

        protected virtual void SetOutput(TUseCaseOutput output) => Output = output;
    }

    public abstract class UseCase<TUseCaseInput, TUseCaseOutput> : UseCase<TUseCaseOutput>
        where TUseCaseInput : class, IUseCaseInput
        where TUseCaseOutput : class, IUseCaseOutput
    {
        protected TUseCaseInput Input;

        public Task ExecuteAsync<TInputData>(TUseCaseOutput output, TInputData data)
        {
            SetInput(data);
            SetOutput(output);

            return Execute();
        }

        protected abstract void SetInput<TInputData>(TInputData data);
    }
}