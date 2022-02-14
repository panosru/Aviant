namespace Aviant.Core.Identity.Entities;

using Core.Entities;

/// <inheritdoc />
/// <summary>
///     Adds the user id who created the entity
/// </summary>
public interface ICreationAudited : IHasCreationTime
{
    /// <summary>
    ///     The id of the user who created the entity
    /// </summary>
    public Guid CreatedBy { get; set; }
}

/// <inheritdoc />
/// <summary>
///     Adds the user id who last modified the entity
/// </summary>
public interface IModificationAudited : IHasModificationTime
{
    /// <summary>
    ///     The id of the user who last modified the entity
    /// </summary>
    public Guid? LastModifiedBy { get; set; }
}

/// <inheritdoc />
/// <summary>
///     Adds the user id who deleted the entity.
///     Works when the entity implements <see cref="T:Aviant.Core.Entities.ISoftDelete" />
/// </summary>
public interface IDeletionAudited : IHasDeletionTime
{
    /// <summary>
    ///     The id of the user who deleted the entity
    /// </summary>
    public Guid? DeletedBy { get; set; }
}
