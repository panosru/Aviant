namespace Aviant.Foundation.Core.Entities;

using System.Globalization;

/// <inheritdoc />
public abstract class Entity<TKey> : IEntity<TKey>
{
    #pragma warning disable 8618
    /// <summary>
    ///     Constructor required by reflection
    /// </summary>
    protected Entity()
    { }
    #pragma warning restore 8618

    /// <summary>
    ///     Entity constructor
    /// </summary>
    /// <param name="id">the id of the entity</param>
    protected Entity(TKey id) => Id = id;

    #region IEntity<TKey> Members

    /// <inheritdoc />
    /// <summary>
    ///     The id of the entity
    /// </summary>
    public TKey Id { get; set; }

    /// <inheritdoc />
    /// <summary>
    ///     Check if the entity is transient (basically if the entity has a preset id)
    /// </summary>
    /// <returns>The id or false if there isn’t any</returns>
    public bool IsTransient()
    {
        if (EqualityComparer<TKey>.Default.Equals(Id, default))
            return true;

        // Workaround for EF Core since it sets int/long to min value when attaching to DbContext
        if (typeof(TKey) == typeof(int))
            return Convert.ToInt32(Id, CultureInfo.InvariantCulture) <= 0;

        if (typeof(TKey) == typeof(long))
            return Convert.ToInt64(Id, CultureInfo.InvariantCulture) <= 0;

        return false;
    }

    /// <inheritdoc />
    public virtual Task<bool> ValidateAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult(true);

    #endregion

    #region Equality Check

    /// <summary>
    ///     Override the default equality functionality for entities
    /// </summary>
    /// <param name="obj">The entity object this object is being checked against</param>
    /// <returns>boolean</returns>
    public override bool Equals(object? obj) =>
        obj is Entity<TKey> entity
     && GetType() == entity.GetType()
     && EqualityComparer<TKey>.Default.Equals(Id, entity.Id);

    // ReSharper disable once NonReadonlyMemberInGetHashCode
    /// <summary>
    ///     Override the default GetHashCode for entity object
    /// </summary>
    /// <returns>The hash code in integer type</returns>
    public override int GetHashCode() =>
        HashCode.Combine(GetType(), Id);

    /// <summary>
    ///     Override equal operator
    /// </summary>
    /// <param name="left">The entity object of the left side</param>
    /// <param name="right">The entity object of the right side</param>
    /// <returns>boolean</returns>
    public static bool operator ==(Entity<TKey> left, Entity<TKey> right) =>
        EqualityComparer<Entity<TKey>>.Default.Equals(left, right);

    /// <summary>
    ///     Override not equal operator
    /// </summary>
    /// <param name="left">The entity object of the left side</param>
    /// <param name="right">The entity object of the right side</param>
    /// <returns>boolean</returns>
    public static bool operator !=(Entity<TKey> left, Entity<TKey> right) =>
        !(left == right);

    #endregion
}
