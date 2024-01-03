using FluentValidation;
using FluentValidation.Results;
using MediatR;
using ValidationException = Aviant.Application.Exceptions.ValidationException;

namespace Aviant.Application.Behaviours;

/// <inheritdoc />
/// <summary>
///     Validation behaviour injected into request pipeline
/// </summary>
/// <typeparam name="TRequest">The expected request type</typeparam>
/// <typeparam name="TResponse">The expected response type</typeparam>
public sealed class ValidationBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    ///     The constructor of the validation behaviour object
    /// </summary>
    /// <param name="validators">Get the validators for the current request object from service container</param>
    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators;

    #region IPipelineBehavior<TRequest,TResponse> Members

    /// <inheritdoc />
    /// <summary>
    ///     Handles the validation behaviour
    /// </summary>
    /// <param name="request">The request object</param>
    /// <param name="next">The next request handler delegate</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public async Task<TResponse> Handle(
        TRequest          request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next().ConfigureAwait(false);

        await new ValidationProcessor<TRequest>(_validators, request)
           .HandleValidationAsync(cancellationToken)
           .ConfigureAwait(false);

        return await next().ConfigureAwait(false);
    }

    #endregion
}

internal sealed class ValidationProcessor<TInput>
    where TInput : class
{
    private readonly ValidationContext<TInput> _context;

    private readonly IEnumerable<IValidator<TInput>> _validators;

    public ValidationProcessor(
        IEnumerable<IValidator<TInput>> validators,
        TInput                          input)
    {
        _validators = validators;
        _context    = new ValidationContext<TInput>(input);
    }

    public async Task HandleValidationAsync(CancellationToken cancellationToken)
    {
        ValidationResult[] validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(_context, cancellationToken)))
           .ConfigureAwait(false);

        List<ValidationFailure> failures = validationResults.SelectMany(r => r.Errors)
           .Where(f => f is not null)
           .ToList();

        if (0 != failures.Count)
            throw new ValidationException(failures);
    }
}
