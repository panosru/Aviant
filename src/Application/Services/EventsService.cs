namespace Aviant.DDD.Application.Services
{
    #region

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Aggregates;
    using Domain.EventBus;
    using Domain.Persistence;
    using Domain.Services;

    #endregion

    public class EventsService<TAggregate, TAggregateId> : IEventsService<TAggregate, TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        private readonly IEventProducer<TAggregate, TAggregateId> _eventProducer;

        private readonly IEventsRepository<TAggregate, TAggregateId> _eventsRepository;

        public EventsService(
            IEventsRepository<TAggregate, TAggregateId> eventsRepository,
            IEventProducer<TAggregate, TAggregateId>    eventProducer)
        {
            _eventsRepository = eventsRepository ?? throw new ArgumentNullException(nameof(eventsRepository));

            _eventProducer = eventProducer ?? throw new ArgumentNullException(nameof(eventProducer));
        }

        #region IEventsService<TAggregate,TAggregateId> Members

        public async Task PersistAsync(TAggregate aggregate)
        {
            if (aggregate is null)
                throw new ArgumentNullException(nameof(aggregate));

            if (!aggregate.Events.Any())
                return;

            await _eventsRepository.AppendAsync(aggregate);
            await _eventProducer.DispatchAsync(aggregate);
        }

        public Task<TAggregate> RehydrateAsync(TAggregateId key) => _eventsRepository.RehydrateAsync(key);

        #endregion
    }
}