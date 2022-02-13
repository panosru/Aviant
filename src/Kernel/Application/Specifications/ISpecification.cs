namespace Aviant.Application.Specifications;

using System.Linq.Expressions;

/// <summary>
/// Represents that the implemented classes are specifications. For more
/// information about the specification pattern, please refer to
/// http://martinfowler.com/apsupp/spec.pdf.
/// </summary>
/// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
public interface ISpecification<T>
{
    /// <summary>
    /// Returns a <see cref="bool"/> value which indicates whether
    /// the specification is satisfied by the given object.
    /// </summary>
    /// <param name="obj">The object to which the specification is applied.</param>
    /// <returns>True if the specification is satisfied, otherwise false.</returns>
    bool IsSatisfiedBy(T obj);

    ISpecification<T> All(params ISpecification<T>[] specification);

    ISpecification<T> Any(params ISpecification<T>[] specification);

    ISpecification<T> None(params ISpecification<T>[] specification);

    ISpecification<T> And(ISpecification<T> specification);

    ISpecification<T> Or(ISpecification<T> specification);

    ISpecification<T> Not(ISpecification<T> specification);

    ISpecification<T> AndNot(ISpecification<T> specification);

    ISpecification<T> OrNot(ISpecification<T> specification);

    /// <summary>
    /// Gets the LINQ expression which represents the current specification.
    /// </summary>
    /// <returns>The LINQ expression.</returns>
    Expression<Func<T, bool>> ToExpression();
}
