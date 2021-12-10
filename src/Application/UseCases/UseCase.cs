// ReSharper disable MemberCanBePrivate.Global

#pragma warning disable 8618
namespace Aviant.DDD.Application.UseCases;

using Behaviours;
using Core.Aggregates;
using Core.Services;
using FluentValidation;
using Orchestration;
using Persistance;

/// <summary>
///     The Use Case Abstract Base class
/// </summary>
/// <typeparam name="TUseCaseOutput">The expected output object type</typeparam>
public abstract class UseCaseBase<TUseCaseOutput>
    : IUseCase<TUseCaseOutput>
    where TUseCaseOutput : class, IUseCaseOutput
{
    /// <summary>
    ///     The output object
    /// </summary>
    protected TUseCaseOutput Output;

    /// <summary>
    ///     The orchestrator object
    /// </summary>
    protected IOrchestrator Orchestrator =>
        ServiceLocator.ServiceContainer.GetRequiredService<IOrchestrator>(
            typeof(IOrchestrator));

    #region IUseCase<TUseCaseOutput> Members

    /// <summary>
    ///     Sets the Output object
    /// </summary>
    /// <param name="output">The output object</param>
    public void SetOutput(TUseCaseOutput output) => Output = output;

    #endregion
}

/// <inheritdoc cref="Aviant.DDD.Application.UseCases.UseCaseBase&lt;TUseCaseOutput&gt;" />
/// <inheritdoc cref="Aviant.DDD.Application.UseCases.IUseCaseExecute" />
/// <summary>
///     UseCase abstract class (without input data)
/// </summary>
/// <typeparam name="TUseCaseOutput">The expected output object type</typeparam>
public abstract class UseCase<TUseCaseOutput>
    : UseCaseBase<TUseCaseOutput>,
      IUseCaseExecute
    where TUseCaseOutput : class, IUseCaseOutput
{
    #region IUseCaseExecute Members

    /// <summary>
    ///     Executes the current use case asynchronously
    /// </summary>
    /// <param name="cancellationToken">The cancellation token object</param>
    /// <returns></returns>
    public abstract Task ExecuteAsync(CancellationToken cancellationToken = default);

    #endregion
}

/// <inheritdoc cref="Aviant.DDD.Application.UseCases.UseCaseBase&lt;TUseCaseOutput&gt;" />
/// <inheritdoc cref="Aviant.DDD.Application.UseCases.IUseCaseExecute&lt;TUseCaseInput&gt;" />
/// <summary>
///     UseCase abstract class with input and output
/// </summary>
/// <typeparam name="TUseCaseInput"></typeparam>
/// <typeparam name="TUseCaseOutput"></typeparam>
public abstract class UseCase<TUseCaseInput, TUseCaseOutput>
    : UseCaseBase<TUseCaseOutput>,
      IUseCaseExecute<TUseCaseInput>
    where TUseCaseInput : class, IUseCaseInput
    where TUseCaseOutput : class, IUseCaseOutput
{
    #region IUseCaseExecute<TUseCaseInput> Members

    /// <summary>
    ///     Execute the current use case
    /// </summary>
    /// <param name="input">The input data object</param>
    /// <param name="cancellationToken">The cancellation token object</param>
    /// <returns></returns>
    public abstract Task ExecuteAsync(TUseCaseInput input, CancellationToken cancellationToken = default);

    #endregion

    /// <summary>
    ///     Run validation rules for current input object
    /// </summary>
    /// <param name="input">The input object</param>
    /// <param name="cancellationToken">The cancellation token object</param>
    /// <returns></returns>
    protected virtual Task ValidateInputAsync(
        TUseCaseInput     input,
        CancellationToken cancellationToken = default) =>
        new ValidationProcessor<TUseCaseInput>(
                ServiceLocator.ServiceContainer
                   .GetRequiredService<IEnumerable<IValidator<TUseCaseInput>>>(
                        typeof(IEnumerable<IValidator<TUseCaseInput>>)),
                input)
           .HandleValidationAsync(cancellationToken);
}

/// <inheritdoc />
/// <summary>
///     UseCase abstract class with input, output and DbContext
/// </summary>
/// <typeparam name="TUseCaseInput">The expected input type</typeparam>
/// <typeparam name="TUseCaseOutput">The expected output type</typeparam>
/// <typeparam name="TDbContext">The expected DbContext type</typeparam>
public abstract class UseCase<TUseCaseInput, TUseCaseOutput, TDbContext>
    : UseCase<TUseCaseInput, TUseCaseOutput>
    where TUseCaseInput : class, IUseCaseInput
    where TUseCaseOutput : class, IUseCaseOutput
    where TDbContext : IDbContextWrite
{
    /// <summary>
    ///     The orchestrator object
    /// </summary>
    protected new IOrchestrator<TDbContext> Orchestrator =>
        ServiceLocator.ServiceContainer.GetRequiredService<IOrchestrator<TDbContext>>(
            typeof(IOrchestrator<TDbContext>));
}

/// <inheritdoc />
/// <summary>
///     UseCase abstract class with input, output, aggregate and aggregate id
/// </summary>
/// <typeparam name="TUseCaseInput">The expected input type</typeparam>
/// <typeparam name="TUseCaseOutput">The expected output type</typeparam>
/// <typeparam name="TAggregate">The expected aggregate type</typeparam>
/// <typeparam name="TAggregateId">The expected aggregate id type</typeparam>
public abstract class UseCase<TUseCaseInput, TUseCaseOutput, TAggregate, TAggregateId>
    : UseCase<TUseCaseInput, TUseCaseOutput>
    where TUseCaseInput : class, IUseCaseInput
    where TUseCaseOutput : class, IUseCaseOutput
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    /// <summary>
    ///     The orchestrator object
    /// </summary>
    protected new IOrchestrator<TAggregate, TAggregateId> Orchestrator =>
        ServiceLocator.ServiceContainer.GetRequiredService<IOrchestrator<TAggregate, TAggregateId>>(
            typeof(IOrchestrator<TAggregate, TAggregateId>));
}
