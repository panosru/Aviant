using System.Linq.Expressions;

namespace Aviant.Core.Linq.Extensions;

/// <summary>
///     Some useful extension methods for <see cref="IQueryable" />.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    ///     Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
    /// </summary>
    public static IQueryable<T> PageBy<T>(
        this IQueryable<T> query,
        int                skipCount,
        int                maxResultCount)
    {
        if (query is null)
            throw new ArgumentNullException(nameof(query));

        return query.Skip(skipCount).Take(maxResultCount);
    }

    /// <summary>
    ///     Filters a <see cref="IQueryable{T}" /> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition" /></returns>
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T>        query,
        bool                      condition,
        Expression<Func<T, bool>> predicate) => condition
        ? query.Where(predicate)
        : query;

    /// <summary>
    ///     Filters a <see cref="IQueryable{T}" /> by given predicate if given condition is true.
    /// </summary>
    /// <param name="query">Queryable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the query</param>
    /// <returns>Filtered or not filtered query based on <paramref name="condition" /></returns>
    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T>             query,
        bool                           condition,
        Expression<Func<T, int, bool>> predicate) => condition
        ? query.Where(predicate)
        : query;
}
