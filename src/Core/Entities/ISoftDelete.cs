namespace Aviant.DDD.Core.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}