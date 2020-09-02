namespace Aviant.DDD.Application
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Behaviours;
    using FluentValidation;
    using MediatR;
    using Microsoft.Extensions.DependencyInjection;
    using Processors;
    using Scrutor;

    public static class DependencyInjection
    {
        private static readonly HashSet<Type> Decorators;

        static DependencyInjection()
        {
            Decorators = new HashSet<Type>(
                new[]
                {
                    typeof(RetryProcessor<>)
                });
        }

        public static IServiceCollection RegisterApplication(
            this IServiceCollection services,
            Assembly                domainAssembly,
            Assembly?               applicationAssembly = null)
        {
            applicationAssembly ??= Assembly.GetCallingAssembly();

            services.AddMediatR(applicationAssembly);

            services.AddValidatorsFromAssembly(applicationAssembly);

            services.Scan(
                scan =>
                {
                    scan.FromAssemblies(domainAssembly)
                        .RegisterHandlers(typeof(IRequestHandler<>))
                        .RegisterHandlers(typeof(IRequestHandler<,>))
                        .RegisterHandlers(typeof(INotificationHandler<>));
                });

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(PerformanceBehaviour<,>));

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehaviour<,>));

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(UnhandledExceptionBehaviour<,>));

            return services;
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