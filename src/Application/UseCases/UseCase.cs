// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable 8618
namespace Aviant.DDD.Application.UseCases
{
    using System.Data;
    using System.Threading.Tasks;
    using Core.Services;
    using Orchestration;

    public abstract class UseCase<TUseCaseInput, TUseCaseOutput> : IUseCase<TUseCaseOutput>
        where TUseCaseInput : class, IUseCaseInput
        where TUseCaseOutput : class, IUseCaseOutput
    {
        protected TUseCaseInput Input;

        protected TUseCaseOutput Output;

        protected IOrchestrator Orchestrator
        {
            get
            {
                if (ServiceLocator.ServiceContainer is null)
                    throw new NoNullAllowedException(typeof(ServiceLocator).FullName);
                
                return ServiceLocator.ServiceContainer.GetService<IOrchestrator>(typeof(IOrchestrator));
            }
        }
        
        public Task Execute<TInputData>(TUseCaseOutput output, TInputData data)
            where TInputData : class
        {
            SetInput(data);
            SetOutput(output);

            Execute();
            
            return Task.CompletedTask;
        }

        protected abstract Task Execute();

        protected abstract void SetInput<TInputData>(TInputData data);

        protected virtual void SetOutput(TUseCaseOutput output) => Output = output;
    }
}