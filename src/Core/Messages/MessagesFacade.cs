namespace Aviant.DDD.Core.Messages;

using Exceptions;
using Services;

internal static class MessagesFacade
{
    [ThreadStatic]
    private static IMessages? _mockContainer;

    private static bool _fromTesting;

    public static void SetTestingEnvironment()
    {
        _fromTesting = true;
    }

    /// <summary>
    ///     This method should be used only for testing purpose
    ///     Under normal use the container is obtained via DI
    /// </summary>
    /// <param name="mockContainer"></param>
    /// <exception cref="Exception"></exception>
    public static void SetMessagesContainer(IMessages mockContainer)
    {
        if (!_fromTesting)
            throw new CoreException(
                @"For SetMessagesContainer to work properly SetTestingEnvironment() should be called first. 
                                      This method should be used only for testing purpose");

        _mockContainer = mockContainer;
    }

    private static IMessages? GetContainer() => _fromTesting
        ? _mockContainer
        : ServiceLocator.ServiceContainer.GetRequiredService<IMessages>(
            typeof(IMessages));

    public static void AddMessage(string message)
    {
        var container = GetContainer();
        container?.AddMessage(message);
    }

    public static List<string>? GetAll() => GetContainer()?.GetAll();

    public static bool HasMessages()
    {
        var container = GetContainer();

        return container?.HasMessages() == true;
    }
}
