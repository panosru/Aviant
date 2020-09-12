namespace Aviant.DDD.Domain.Entities
{
    #region

    using System.Threading.Tasks;

    #endregion

    public interface IEntity<out TKey>
    {
        TKey Id { get; }

        Task<bool> Validate();
    }
}