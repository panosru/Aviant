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

        public EventConsumerFactory(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public IEventConsumer Build<TAggregateRoot, TKey>()
            where TAggregateRoot : IAggregateRoot<TKey>
        {
            using var scope = _scopeFactory.CreateScope();
            var consumer = scope.ServiceProvider.GetRequiredService<IEventConsumer<TAggregateRoot, TKey>>();

            async Task onEventReceived(object s, IEvent<TKey> @event)
            {
                var constructedEvent = EventReceivedFactory.Create((dynamic) @event);

                using var innerScope = _scopeFactory.CreateScope();
                //TODO: User Orchestrator
                var mediator = innerScope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Publish(constructedEvent, CancellationToken.None);
            }

            consumer.EventReceived += onEventReceived;

            return consumer;
        }
    }
}