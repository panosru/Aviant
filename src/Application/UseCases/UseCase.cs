// ReSharper disable MemberCanBePrivate.Global

#pragma warning disable 8618
namespace Aviant.DDD.Application.UseCases
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Aggregates;
    using Core.Services;
    using Orchestration;
    using Persistance;

    public abstract class UseCaseBase<TUseCaseOutput>
        : IUseCase<TUseCaseOutput>
        where TUseCaseOutput : class, IUseCaseOutput
    {
        protected TUseCaseOutput Output;

        protected IOrchestrator Orchestrator =>
            ServiceLocator.ServiceContainer.GetRequiredService<IOrchestrator>(
                typeof(IOrchestrator));

        #region IUseCase<TUseCaseOutput> Members

        public void SetOutput(TUseCaseOutput output) => Output = output;

        #endregion
    }

    public abstract class UseCase<TUseCaseOutput>
        : UseCaseBase<TUseCaseOutput>,
          IUseCaseExecute
        where TUseCaseOutput : class, IUseCaseOutput
    {
        #region IUseCaseExecute Members

        public abstract Task Execute(CancellationToken cancellationToken = default);

        #endregion
    }

    public abstract class UseCase<TUseCaseInput, TUseCaseOutput>
        : UseCaseBase<TUseCaseOutput>,
          IUseCaseExecute<TUseCaseInput>
        where TUseCaseInput : class, IUseCaseInput
        where TUseCaseOutput : class, IUseCaseOutput
    {
        #region IUseCaseExecute<TUseCaseInput> Members

        public abstract Task Execute(TUseCaseInput input, CancellationToken cancellationToken = default);

        #endregion
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