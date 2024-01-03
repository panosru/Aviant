using System.Linq.Expressions;

namespace Aviant.Application.Specifications;

/// <inheritdoc />
/// <summary>
/// Represents the specification that can be satisfied if any of the given specifications can be satisfied.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public sealed class AnySpecification<T> : Specification<T>
{
    private readonly ISpecification<T>[] _specifications;

    public AnySpecification(params ISpecification<T>[] specifications) => _specifications = specifications;

    /// <inheritdoc />
    /// <summary>
    /// Gets the LINQ expression which represents the current specification.
    /// </summary>
    /// <returns>The LINQ expression.</returns>
    public override Expression<Func<T, bool>> ToExpression() => candidate =>
        _specifications.ToList().Exists(specification => specification.IsSatisfiedBy(candidate));
}
