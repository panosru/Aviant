using MediatR;

namespace Aviant.Core.EventSourcing.EventBus;

public sealed record EventReceived<TDomainEvent>(TDomainEvent Event) : INotification;
