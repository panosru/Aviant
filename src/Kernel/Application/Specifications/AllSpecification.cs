namespace Aviant.Application.Specifications;

using System.Linq.Expressions;

/// <inheritdoc />
/// <summary>
/// Represents the specification that can be satisfied if all of the given specifications are satisfied.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public sealed class AllSpecification<T> : Specification<T>
{
    private readonly ISpecification<T>[] _specifications;

    /// <inheritdoc />
    public AllSpecification(params ISpecification<T>[] specifications) => _specifications = specifications;

    /// <inheritdoc />
    /// <summary>
    /// Gets the LINQ expression which represents the current specification.
    /// </summary>
    /// <returns>The LINQ expression.</returns>
    public override Expression<Func<T, bool>> ToExpression() => candidate =>
        _specifications.All(specification => specification.IsSatisfiedBy(candidate));
}
