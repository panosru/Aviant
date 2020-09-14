namespace Aviant.DDD.Core.Services
{
    public static class ServiceLocator
    {
        public static IServiceContainer? ServiceContainer { get; private set; }

        public static void Initialise(IServiceContainer serviceContainer)
        {
            ServiceContainer = serviceContainer;
        }
    }
}