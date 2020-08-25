namespace Aviant.DDD.Domain.EventBus
{
    using System.Threading;
    using System.Threading.Tasks;
    using Aggregates;
    using Events;

    public interface IEventConsumer
    {
        Task ConsumeAsync(CancellationToken cancellationToken);
    }

    public interface IEventConsumer<TAggregateRoot, out TKey> : IEventConsumer
        where TAggregateRoot : IAggregateRoot<TKey>
    {
        event EventReceivedHandler<TKey> EventReceived;
    }

    public delegate Task EventReceivedHandler<in TKey>(object sender, IEvent<TKey> @event);
}