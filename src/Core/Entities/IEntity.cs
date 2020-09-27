namespace Aviant.DDD.Core.Entities
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEntity<out TKey>
    {
        public TKey Id { get; }

        public Task<bool> Validate(CancellationToken cancellationToken = default);
    }
}