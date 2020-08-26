namespace Aviant.DDD.Application.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Aggregates;
    using Domain.EventBus;
    using Domain.Persistence;
    using Domain.Services;

    public class EventsService<TAggregateRoot, TKey> : IEventsService<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        private readonly IEventProducer<TAggregateRoot, TKey> _eventProducer;
        private readonly IEventsRepository<TAggregateRoot, TKey> _eventsRepository;

        public EventsService(
            IEventsRepository<TAggregateRoot, TKey> eventsRepository,
            IEventProducer<TAggregateRoot, TKey> eventProducer)
        {
            _eventsRepository = eventsRepository ??
                                throw new ArgumentNullException(nameof(eventsRepository));

            _eventProducer = eventProducer ??
                             throw new ArgumentNullException(nameof(eventProducer));
        }

        public async Task PersistAsync(TAggregateRoot aggregateRoot)
        {
            if (aggregateRoot is null)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            await _eventsRepository.AppendAsync(aggregateRoot);
            await _eventProducer.DispatchAsync(aggregateRoot);
        }

        public Task<TAggregateRoot> RehydrateAsync(TKey key)
        {
            return _eventsRepository.RehydrateAsync(key);
        }
    }
}