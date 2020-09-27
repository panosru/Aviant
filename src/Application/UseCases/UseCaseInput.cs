namespace Aviant.DDD.Application.UseCases
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Services;
    using FluentValidation;
    using FluentValidation.Results;

    public abstract class UseCaseInput : IUseCaseInput
    {
        public ValidationResult ValidationResult { get; protected set; } = new ValidationResult();

        #region IUseCaseInput Members

        public virtual Task Validate(CancellationToken cancellationToken = default) => Task.CompletedTask;

        #endregion
    }

    public abstract class UseCaseInput<TInput, TValidator> : UseCaseInput
        where TValidator : IValidator<TInput>
        where TInput : class, IUseCaseInput
    {
        private TValidator Validator { get; } = ServiceLocator.ServiceContainer
           .GetRequiredService<TValidator>(typeof(TValidator));

        protected async Task UseDefaultValidation(
            TInput            input,
            CancellationToken cancellationToken = default) => ValidationResult =
            await Validator.ValidateAsync(
                    input
                 ?? throw new NullReferenceException(typeof(UseCaseInput<TInput, TValidator>).FullName),
                    cancellationToken)
               .ConfigureAwait(false);
    }
}