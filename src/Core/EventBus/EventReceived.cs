namespace Aviant.Core.EventBus;

using MediatR;

public sealed record EventReceived<TDomainEvent>(TDomainEvent Event) : INotification;
