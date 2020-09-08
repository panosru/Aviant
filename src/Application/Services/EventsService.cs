namespace Aviant.DDD.Application.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Aggregates;
    using Domain.EventBus;
    using Domain.Persistence;
    using Domain.Services;

    public class EventsService<TAggregateRoot, TAggregateId> : IEventsService<TAggregateRoot, TAggregateId>
        where TAggregateRoot : class, IAggregateRoot<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        private readonly IEventProducer<TAggregateRoot, TAggregateId> _eventProducer;

        private readonly IEventsRepository<TAggregateRoot, TAggregateId> _eventsRepository;

        public EventsService(
            IEventsRepository<TAggregateRoot, TAggregateId> eventsRepository,
            IEventProducer<TAggregateRoot, TAggregateId>    eventProducer)
        {
            _eventsRepository = eventsRepository ?? throw new ArgumentNullException(nameof(eventsRepository));

            _eventProducer = eventProducer ?? throw new ArgumentNullException(nameof(eventProducer));
        }

        #region IEventsService<TAggregateRoot,TAggregateId> Members

        public async Task PersistAsync(TAggregateRoot aggregateRoot)
        {
            if (aggregateRoot is null)
                throw new ArgumentNullException(nameof(aggregateRoot));

            if (!aggregateRoot.Events.Any())
                return;

            await _eventsRepository.AppendAsync(aggregateRoot);
            await _eventProducer.DispatchAsync(aggregateRoot);
        }

        public Task<TAggregateRoot> RehydrateAsync(TAggregateId key) => _eventsRepository.RehydrateAsync(key);

        #endregion
    }
}