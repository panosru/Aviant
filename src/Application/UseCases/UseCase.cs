// ReSharper disable MemberCanBePrivate.Global

#pragma warning disable 8618
namespace Aviant.DDD.Application.UseCases
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Core.Aggregates;
    using Core.Services;
    using Orchestration;
    using Persistance;

    public abstract class UseCase<TUseCaseOutput> : IUseCase<TUseCaseOutput>
        where TUseCaseOutput : class, IUseCaseOutput
    {
        protected TUseCaseOutput Output;

        protected IOrchestrator Orchestrator =>
            ServiceLocator.ServiceContainer.GetRequiredService<IOrchestrator>(
                typeof(IOrchestrator));
        
        public abstract Task Execute();

        public         void SetOutput(TUseCaseOutput output) => Output = output;
    }

    public abstract class UseCase<TUseCaseInput, TUseCaseOutput> : UseCase<TUseCaseOutput>
        where TUseCaseInput : class, IUseCaseInput
        where TUseCaseOutput : class, IUseCaseOutput
    {
        protected TUseCaseInput Input;
    }
    
    public abstract class UseCase<TUseCaseInput, TUseCaseOutput, TDbContext> 
        : UseCase<TUseCaseInput, TUseCaseOutput>
        where TUseCaseInput : class, IUseCaseInput
        where TUseCaseOutput : class, IUseCaseOutput
        where TDbContext : IDbContextWrite
    {
        protected new IOrchestrator<TDbContext> Orchestrator =>
            ServiceLocator.ServiceContainer.GetRequiredService<IOrchestrator<TDbContext>>(
                typeof(IOrchestrator<TDbContext>));
    }
    
    public abstract class UseCase<TUseCaseInput, TUseCaseOutput, TAggregate, TAggregateId> 
        : UseCase<TUseCaseInput, TUseCaseOutput>
        where TUseCaseInput : class, IUseCaseInput
        where TUseCaseOutput : class, IUseCaseOutput
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        protected new IOrchestrator<TAggregate, TAggregateId> Orchestrator =>
            ServiceLocator.ServiceContainer.GetRequiredService<IOrchestrator<TAggregate, TAggregateId>>(
                typeof(IOrchestrator<TAggregate, TAggregateId>));
    }
}