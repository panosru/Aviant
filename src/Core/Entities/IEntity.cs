namespace Aviant.DDD.Core.Entities
{
    using System.Threading.Tasks;

    public interface IEntity<out TKey>
    {
        TKey Id { get; }

        Task<bool> Validate();
    }
}