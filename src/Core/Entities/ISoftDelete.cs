namespace Aviant.DDD.Core.Entities
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}