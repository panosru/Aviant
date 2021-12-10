namespace Aviant.DDD.Core.Aggregates;

/// <summary>
///     The interface to define the AggregateId object
/// </summary>
public interface IAggregateId
{
    /// <summary>
    ///     Serialises the aggregate id object
    /// </summary>
    /// <returns>The aggregate id in byte[] type</returns>
    public byte[] Serialize();
}

/// <inheritdoc />
internal interface IAggregateId<out T> : IAggregateId
    where T : notnull
{
    public T Key { get; }
}
