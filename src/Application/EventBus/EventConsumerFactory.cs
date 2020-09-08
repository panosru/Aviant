namespace Aviant.DDD.Application.EventBus
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain.Aggregates;
    using Domain.EventBus;
    using Domain.Events;
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

            async Task OnEventReceived(object s, IEvent<TAggregateId> @event)
            {
                var constructedEvent = EventReceivedFactory.Create((dynamic) @event);

                using var innerScope = _scopeFactory.CreateScope();
                var       mediator   = innerScope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(constructedEvent, CancellationToken.None);
            }

            consumer.EventReceived += OnEventReceived;

            return consumer;
        }

        #endregion
    }
}