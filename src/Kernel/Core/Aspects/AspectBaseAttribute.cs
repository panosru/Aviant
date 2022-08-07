namespace Aviant.Core.Aspects;

using AspectCore.DynamicProxy;

[AttributeUsage(
    AttributeTargets.Class
  | AttributeTargets.Enum
  | AttributeTargets.Interface
  | AttributeTargets.Delegate)]
public abstract class AspectBaseAttribute : AbstractInterceptorAttribute
{
    /// <inheritdoc />
    public override async Task Invoke(AspectContext context, AspectDelegate next)
    {
        try
        {
            Pre(context);
            await next(context);
        }
        catch (Exception exception)
        {
            Error(exception);
        }
        finally
        {
            Post(context);
        }
    }

    protected virtual void Pre(AspectContext context)
    { }

    protected virtual void Post(AspectContext context)
    { }

    protected virtual void Error(Exception exception) => throw exception;
}
