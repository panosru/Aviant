namespace Aviant.Core.Exceptions;

public static class Try
{
    public static void Silently(
        Action            callback,
        Action<Exception> onException,
        Action?           finallyCallback = null)
    {
        try
        {
            callback();
        }
        catch (Exception exception)
        {
            onException(exception);
        }
        finally
        {
            finallyCallback?.Invoke();
        }
    }

    public static async Task SilentlyAsync(
        Func<Task>            callback,
        Func<Exception, Task> onException,
        Func<Task>?           finallyCallback = null)
    {
        try
        {
            await callback()
               .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            await onException(ex)
               .ConfigureAwait(false);
        }
        finally
        {
            if (finallyCallback is not null)
                await finallyCallback()
                   .ConfigureAwait(false);
        }
    }
}
