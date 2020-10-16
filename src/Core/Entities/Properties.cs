namespace Aviant.DDD.Core.Entities
{
    /// <summary>
    ///     Enables the entity to not be hard deleted from the database
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        ///     The status of the object, if it is marked as deleted or not
        /// </summary>
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    ///     Allowing you to set the activation status of the entity.
    /// </summary>
    public interface IHasActivationStatus
    {
        /// <summary>
        ///     The activation status of the entity
        /// </summary>
        public bool IsActive { get; set; }
    }
}