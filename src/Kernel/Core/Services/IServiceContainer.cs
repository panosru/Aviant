namespace Aviant.Core.Services;

public interface IServiceContainer
{
    public object GetRequiredService(Type type);

    public T GetRequiredService<T>(Type type);

    public object GetService(Type type);

    public T GetService<T>(Type type);
}
