namespace Aviant.Foundation.Core.Entities;

/// <summary>
///     Defines interface for base entity type. All entities in the system must implement this interface.
/// </summary>
/// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
public interface IEntity<out TKey>
{
    /// <summary>
    ///     Unique identifier for this entity.
    /// </summary>
    public TKey Id { get; }

    /// <summary>
    ///     Checks if this entity is transient (not persisted to database and it has not an <see cref="Id" />).
    /// </summary>
    /// <returns>True, if this entity is transient</returns>
    public bool IsTransient();

    /// <summary>
    ///     Run validations against the entity
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Boolean task, depending if the validation pass or fail</returns>
    public Task<bool> ValidateAsync(CancellationToken cancellationToken = default);
}
