// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable 8618
namespace Aviant.DDD.Application.UseCases
{
    using System.Data;
    using System.Threading.Tasks;
    using Core.Services;
    using Orchestration;

    public abstract class UseCase<TUseCaseOutput> : IUseCase<TUseCaseOutput>
        where TUseCaseOutput : class, IUseCaseOutput
    {
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

        public new Task Execute<TInputData>(TUseCaseOutput output, TInputData data)
            where TInputData : class
        {
            SetInput(data);
            SetOutput(output);

            Execute();
            
            return Task.CompletedTask;
        }

        protected abstract void SetInput<TInputData>(TInputData data);
    }
}