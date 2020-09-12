namespace Aviant.DDD.Application.EventBus
{
    #region

    using System;
    using System.Collections.Generic;
    using Processors;
    using Scrutor;

    #endregion

    public static class HandlerExtensionRegistry
    {
        private static readonly HashSet<Type> Decorators;

        static HandlerExtensionRegistry()
        {
            Decorators = new HashSet<Type>(
                new[]
                {
                    typeof(RetryProcessor<>)
                });
        }

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