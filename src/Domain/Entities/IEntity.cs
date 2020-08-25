namespace Aviant.DDD.Domain.Entities
{
    using System.Threading.Tasks;

    public interface IEntity<out TKey>
    {
        TKey Id { get; }

        Task<bool> Validate();
    }
}