namespace Aviant.Core.Exceptions;

using System.Runtime.Serialization;

/// <summary>
///     This exception is thrown if an entity excepted to be found but not found.
/// </summary>
[Serializable]
public sealed class EntityNotFoundException : CoreException
{
    /// <summary>
    ///     Creates a new <see cref="EntityNotFoundException" /> object.
    /// </summary>
    public EntityNotFoundException()
    { }

    /// <summary>
    ///     Creates a new <see cref="EntityNotFoundException" /> object.
    /// </summary>
    private EntityNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    { }

    /// <summary>
    ///     Creates a new <see cref="EntityNotFoundException" /> object.
    /// </summary>
    public EntityNotFoundException(
        Type?     entityType,
        object?   id,
        Exception innerException = null!)
        : base($"There is no such an entity. Entity type: {entityType?.FullName}, id: {id}", innerException)
    {
        EntityType = entityType;
        Id         = id;
    }

    /// <summary>
    ///     Creates a new <see cref="EntityNotFoundException" /> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    public EntityNotFoundException(string message)
        : base(message)
    { }

    /// <summary>
    ///     Creates a new <see cref="EntityNotFoundException" /> object.
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    { }

    /// <summary>
    ///     Type of the entity.
    /// </summary>
    public Type? EntityType { get; set; }

    /// <summary>
    ///     Id of the Entity.
    /// </summary>
    public object? Id { get; set; }
}
