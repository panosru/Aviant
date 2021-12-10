namespace Aviant.DDD.Core.Aggregates;

using System.Globalization;
using System.Text;

/// <inheritdoc />
/// <summary>
///     The aggregate id object
/// </summary>
/// <typeparam name="TKey">The key of the aggregate id</typeparam>
public abstract class AggregateId<TKey> : IAggregateId<TKey>
    where TKey : notnull
{
    /// <summary>
    ///     Creates a new instance of AggregateId
    /// </summary>
    /// <param name="key">The key of the aggregate id</param>
    protected AggregateId(TKey key) => Key = key;

    #region IAggregateId<TKey> Members

    /// <summary>
    ///     The key of the aggregate id
    /// </summary>
    public TKey Key { get; }

    /// <inheritdoc />
    public virtual byte[] Serialize() => Encoding.UTF8.GetBytes(ToString());

    #endregion

    /// <summary>
    ///     Returns the key of the aggregate id in string type
    /// </summary>
    /// <returns>The string of the Key</returns>
    public override string ToString()
    {
        return Key switch
        {
            Guid guid => guid.ToString(),
            string id => id,
            int id    => id.ToString(CultureInfo.InvariantCulture),
            _         => (string)(Key as object)
        };
    }
}

