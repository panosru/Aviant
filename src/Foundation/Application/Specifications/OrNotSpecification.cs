namespace Aviant.Foundation.Application.Specifications;

using System.Linq.Expressions;

/// <inheritdoc />
/// <summary>
/// Represents the combined specification which indicates that either the first specification
/// can be satisfied by the given object or the second one cannot.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public class OrNotSpecification<T> : CompositeSpecification<T>
{
    /// <inheritdoc />
    /// <summary>
    /// Constructs a new instance of <see cref="T:Aviant.Foundation.Application.Specifications.OrNotSpecification`1" /> class.
    /// </summary>
    /// <param name="left">The first specification.</param>
    /// <param name="right">The second specification.</param>
    public OrNotSpecification(ISpecification<T> left, ISpecification<T> right)
        : base(left, right)
    { }

    /// <inheritdoc />
    /// <summary>
    /// Gets the LINQ expression which represents the current specification.
    /// </summary>
    /// <returns>The LINQ expression.</returns>
    public override Expression<Func<T, bool>> ToExpression()
    {
        Expression<Func<T, bool>> rightExpression = Right.ToExpression();

        var bodyNot = Expression.Not(rightExpression.Body);

        Expression<Func<T, bool>> bodyNotExpression = Expression.Lambda<Func<T, bool>>(
            bodyNot,
            rightExpression.Parameters);

        return Left.ToExpression().Or(bodyNotExpression);
    }
}
