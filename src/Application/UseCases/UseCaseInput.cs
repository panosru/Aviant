namespace Aviant.DDD.Application.UseCases
{
    using System;
    using Core.Services;
    using FluentValidation;
    using FluentValidation.Results;

    public abstract class UseCaseInput : IUseCaseInput
    {
        public ValidationResult ValidationResult { get; protected set; } = new ValidationResult();
        
        public virtual void Validate()
        { }
    }

    public abstract class UseCaseInput<TInput, TValidator> : UseCaseInput
        where TValidator : IValidator<TInput>
        where TInput : class, IUseCaseInput
    {
        private TValidator Validator { get; } = ServiceLocator.ServiceContainer
           .GetRequiredService<TValidator>(typeof(TValidator));

        protected void UseDefaultValidation(TInput input) => ValidationResult = Validator.Validate(
            input ?? throw new NullReferenceException(typeof(UseCaseInput<TInput, TValidator>).FullName));
    }
}