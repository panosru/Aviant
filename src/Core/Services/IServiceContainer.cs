namespace Aviant.DDD.Core.Services
{
    using System;

    public interface IServiceContainer
    {
        public object GetRequiredService(Type type);

        public T GetRequiredService<T>(Type type);

        public object GetService(Type type);

        public T GetService<T>(Type type);
    }
}
