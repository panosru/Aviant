namespace Aviant.DDD.Domain.Entities
{
    using System.Threading.Tasks;

    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; } = default!;

        public virtual Task<bool> Validate()
        {
            return Task.FromResult(true);
        }
    }
}