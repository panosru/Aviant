namespace Aviant.DDD.Application.EventBus
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Aggregates;
    using Core.EventBus;
    using Core.Events;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;

    public class EventConsumerFactory : IEventConsumerFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public EventConsumerFactory(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

        #region IEventConsumerFactory Members

        public IEventConsumer Build<TAggregate, TAggregateId, TDeserializer>()
            where TAggregate : IAggregate<TAggregateId>
            where TAggregateId : IAggregateId
        {
            using var scope = _scopeFactory.CreateScope();

            var consumer = scope.ServiceProvider.GetRequiredService<IEventConsumer<
                TAggregate, TAggregateId, TDeserializer>>();

            async Task OnEventReceivedAsync(
                object               s,
                IEvent<TAggregateId> @event,
                CancellationToken    cancellationToken = default)
            {
                var constructedEvent = EventReceivedFactory.Create((dynamic) @event);

                using var innerScope = _scopeFactory.CreateScope();
                var       mediator   = innerScope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(constructedEvent, cancellationToken);
            }

            consumer.EventReceived += OnEventReceivedAsync;

            return consumer;
        }

        #endregion
    }
}