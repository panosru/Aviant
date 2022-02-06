namespace Aviant.DDD.Application.Specifications;

using System.Linq.Expressions;

/// <inheritdoc />
/// <summary>
/// Represents the combined specification which indicates that both of the given
/// specifications should be satisfied by the given object.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public class AndSpecification<T> : CompositeSpecification<T>
{
    /// <inheritdoc />
    /// <summary>
    /// Constructs a new instance of <see cref="T:Aviant.DDD.Application.Specifications.AndSpecification`1" /> class.
    /// </summary>
    /// <param name="left">The first specification.</param>
    /// <param name="right">The second specification.</param>
    public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        : base(left, right)
    { }

    /// <inheritdoc />
    /// <summary>
    /// Gets the LINQ expression which represents the current specification.
    /// </summary>
    /// <returns>The LINQ expression.</returns>
    public override Expression<Func<T, bool>> ToExpression() => Left.ToExpression().And(Right.ToExpression());
}
