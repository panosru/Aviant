namespace Aviant.Core.EventSourcing.EventBus;

using MediatR;

public sealed record EventReceived<TDomainEvent>(TDomainEvent Event) : INotification;
