namespace Aviant.DDD.Application.EventBus
{
    using System;
    using System.Collections.Generic;
    using Processors;
    using Scrutor;

    public static class HandlerExtensionRegistry
    {
        private static readonly HashSet<Type> Decorators = new()
        {
            typeof(RetryEventProcessor<>),
            typeof(RetryRequestProcessor<,>)
        };

        public static IImplementationTypeSelector RegisterHandlers(this IImplementationTypeSelector selector, Type type)
        {
            return selector.AddClasses(
                    c =>
                        c.AssignableTo(type)
                           .Where(t => !Decorators.Contains(t))
                )
               .UsingRegistrationStrategy(RegistrationStrategy.Append)
               .AsImplementedInterfaces()
               .WithScopedLifetime();
        }
    }
}