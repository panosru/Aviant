namespace Aviant.DDD.Core.Services
{
    using System;

    public static class ServiceLocator
    {
        private static IServiceProvider? _serviceProvider;

        public static IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider is null)
                    throw new NullReferenceException(typeof(ServiceLocator).FullName);

                return _serviceProvider;
            }
        }

        public static void Initialise(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}