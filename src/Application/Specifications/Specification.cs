namespace Aviant.DDD.Application.Specifications;

using System.Linq.Expressions;

/// <inheritdoc />
/// <summary>
/// Represents the base class for specifications.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public abstract class Specification<T> : ISpecification<T>
{
    /// <inheritdoc />
    /// <summary>
    /// Returns a <see cref="T:System.Boolean" /> value which indicates whether the specification
    /// is satisfied by the given object.
    /// </summary>
    /// <param name="obj">The object to which the specification is applied.</param>
    /// <returns>True if the specification is satisfied, otherwise false.</returns>
    public virtual bool IsSatisfiedBy(T obj) => ToExpression().Compile()(obj);

    /// <inheritdoc />
    public ISpecification<T> All(params ISpecification<T>[] specification) =>
        new AllSpecification<T>(specification);

    /// <inheritdoc />
    public ISpecification<T> Any(params ISpecification<T>[] specification) =>
        new AnySpecification<T>(specification);

    /// <inheritdoc />
    public ISpecification<T> None(params ISpecification<T>[] specification) =>
        new NoneSpecification<T>(specification);

    /// <inheritdoc />
    public ISpecification<T> And(ISpecification<T> specification) =>
        new AndSpecification<T>(this, specification);

    /// <inheritdoc />
    public ISpecification<T> Or(ISpecification<T> specification) =>
        new OrSpecification<T>(this, specification);

    /// <inheritdoc />
    public ISpecification<T> Not(ISpecification<T> specification) =>
        new NotSpecification<T>(specification);

    /// <inheritdoc />
    public ISpecification<T> AndNot(ISpecification<T> specification) =>
        new AndNotSpecification<T>(this, specification);

    /// <inheritdoc />
    public ISpecification<T> OrNot(ISpecification<T> specification) =>
        new OrNotSpecification<T>(this, specification);

    /// <inheritdoc />
    /// <summary>
    /// Gets the LINQ expression which represents the current specification.
    /// </summary>
    /// <returns>The LINQ expression.</returns>
    public abstract Expression<Func<T, bool>> ToExpression();

    /// <summary>
    /// Implicitly converts a specification to expression.
    /// </summary>
    /// <param name="specification"></param>
    public static implicit operator Expression<Func<T, bool>>(Specification<T> specification) =>
        specification.ToExpression();
}
