using Aviant.Core.EventSourcing.Aggregates;
using Aviant.Core.EventSourcing.DomainEvents;
using Aviant.Core.EventSourcing.EventBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Aviant.Application.EventSourcing.EventBus;

public sealed class EventConsumerFactory : IEventConsumerFactory
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

        consumer.EventReceived += OnEventReceivedAsync;

        async Task OnEventReceivedAsync(
            object                     s,
            IDomainEvent<TAggregateId> @event,
            CancellationToken          cancellationToken = default)
        {
            var constructedEvent = EventReceivedFactory.Create((dynamic)@event);

            using var innerScope = _scopeFactory.CreateScope();
            var       mediator   = innerScope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Publish(constructedEvent, cancellationToken);
        }

        return consumer;
    }

    #endregion
}
