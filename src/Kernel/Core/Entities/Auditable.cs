namespace Aviant.Core.Entities;

/// <summary>
///     Enables the auditability for the entity (db record)
/// </summary>
public interface IAuditedEntity
{ }

/// <inheritdoc />
/// <summary>
///     Adds creation time information to the entity
/// </summary>
public interface IHasCreationTime : IAuditedEntity
{
    /// <summary>
    ///     The time the entity was created
    /// </summary>
    public DateTime Created { get; set; }
}

/// <inheritdoc />
/// <summary>
///     Adds last modification time information to the entity
/// </summary>
public interface IHasModificationTime : IAuditedEntity
{
    /// <summary>
    ///     The time the entity was last modified
    /// </summary>
    public DateTime? LastModified { get; set; }
}

/// <inheritdoc />
/// <summary>
///     Adds deletion time information to the entity.
///     Works when the entity implements <see cref="ISoftDelete" />
/// </summary>
public interface IHasDeletionTime : IAuditedEntity
{
    /// <summary>
    ///     The time the entity was deleted
    /// </summary>
    public DateTime? Deleted { get; set; }
}
