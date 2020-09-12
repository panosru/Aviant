namespace Aviant.DDD.Domain.Entities
{
    #region

    using System;

    #endregion

    public interface IAuditedEntity
    { }

    public interface IHasCreationTime : IAuditedEntity
    {
        DateTime Created { get; set; }
    }

    public interface ICreationAudited : IHasCreationTime
    {
        Guid CreatedBy { get; set; }
    }

    public interface IHasModificationTime : IAuditedEntity
    {
        DateTime? LastModified { get; set; }
    }

    public interface IModificationAudited : IHasModificationTime
    {
        Guid? LastModifiedBy { get; set; }
    }

    public interface IHasDeletionTime : IAuditedEntity
    {
        DateTime? Deleted { get; set; }
    }

    public interface IDeletionAudited : IHasDeletionTime
    {
        Guid? DeletedBy { get; set; }
    }

    public interface IHasActivationStatus : IAuditedEntity
    {
        bool IsActive { get; set; }
    }

    public interface IActivationAudited : IHasActivationStatus
    {
        Guid? ActivationStatusModifiedBy { get; set; }
    }
}