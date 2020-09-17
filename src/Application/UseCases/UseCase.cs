// ReSharper disable MemberCanBePrivate.Global

#pragma warning disable 8618
namespace Aviant.DDD.Application.UseCases
{
    using System.Data;
    using System.Threading.Tasks;
    using Core.Services;
    using Microsoft.Extensions.DependencyInjection;
    using Orchestration;

    public abstract class UseCase<TUseCaseOutput> : IUseCase
        where TUseCaseOutput : class, IUseCaseOutput
    {
        protected TUseCaseOutput Output;

        protected IOrchestrator Orchestrator
        {
            get
            {
                if (ServiceLocator.ServiceProvider is null)
                    throw new NoNullAllowedException(typeof(ServiceLocator).FullName);

                return ServiceLocator.ServiceProvider.GetService<IOrchestrator>();
            }
        }

        public Task Execute(TUseCaseOutput output)
        {
            SetOutput(output);

            Execute();

            return Task.CompletedTask;
        }

        protected abstract Task Execute();

        protected virtual void SetOutput(TUseCaseOutput output) => Output = output;
    }

    public abstract class UseCase<TUseCaseInput, TUseCaseOutput> : UseCase<TUseCaseOutput>
        where TUseCaseInput : class, IUseCaseInput
        where TUseCaseOutput : class, IUseCaseOutput
    {
        protected TUseCaseInput Input;

        public Task Execute<TInputData>(TUseCaseOutput output, TInputData data)
        {
            SetInput(data);
            SetOutput(output);

            Execute();

            return Task.CompletedTask;
        }

        protected abstract void SetInput<TInputData>(TInputData data);
    }
}