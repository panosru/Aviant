namespace Aviant.Application.Interceptors;

using System.Collections.Concurrent;
using Castle.DynamicProxy;

public abstract class InterceptorBase<TInterceptor> : IAsyncInterceptor
    where TInterceptor : class, IAsyncInterceptor
{
    /// <inheritdoc />
    public void InterceptSynchronous(IInvocation invocation)
    {
        if (!CanProcess(invocation))
        {
            invocation.Proceed();
            return;
        }

        InterceptorContext context = new() { Invocation = invocation };

        try
        {
            var proceed = context.Invocation?.CaptureProceedInfo()
                       ?? throw new NullReferenceException();

            OnPre(context);

            if (context.FlowBreak)
                return;

            try
            {
                proceed.Invoke();
            }
            catch (Exception e)
            {
                context.Exception = e;
                throw;
            }
            finally
            {
                if (context.Exception is null
                 || (context.Exception is not null
                  && context.ExecutePostEvent))
                    OnPost(context);
            }
        }
        catch (Exception e)
        {
            context.Exception = e;
            OnException(context);

            if (!context.SuppressException)
                throw;
        }
        finally
        {
            OnExit(context);
        }
    }

    /// <inheritdoc />
    public void InterceptAsynchronous(IInvocation invocation)
    {
        if (!CanProcess(invocation))
        {
            invocation.Proceed();
            return;
        }

        InterceptorContext context = new() { Invocation = invocation };
        context.Invocation.ReturnValue = InternalInterceptAsynchronous(context);
    }

    private async Task InternalInterceptAsynchronous(InterceptorContext context)
    {
        try
        {
            var proceed = context.Invocation?.CaptureProceedInfo()
                       ?? throw new NullReferenceException();

            await OnPreAsync(context);

            if (!context.FlowBreak)
                try
                {
                    proceed.Invoke();

                    await ((Task)context.Invocation.ReturnValue)
                       .ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    context.Exception = e;
                    throw;
                }
                finally
                {
                    if (context.Exception is null
                     || (context.Exception is not null
                      && context.ExecutePostEvent))
                        await OnPostAsync(context);
                }
        }
        catch (Exception e)
        {
            context.Exception = e;
            await OnExceptionAsync(context);

            if (!context.SuppressException)
                throw;
        }
        finally
        {
            await OnExitAsync(context);
        }
    }

    /// <inheritdoc />
    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        if (!CanProcess(invocation))
        {
            invocation.Proceed();
            return;
        }

        InterceptorContext context = new() { Invocation = invocation };
        context.Invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(context);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    private async Task<TResult> InternalInterceptAsynchronous<TResult>(InterceptorContext context)
    {
        try
        {
            var proceed = context.Invocation?.CaptureProceedInfo()
                       ?? throw new NullReferenceException();

            await OnPreAsync(context).ConfigureAwait(false);

            if (context.FlowBreak)
                return context.GetResultValue<TResult>();

            try
            {
                proceed.Invoke();

                context.SetResultValue(
                    await ((Task<TResult>)context.Invocation.ReturnValue)
                       .ConfigureAwait(false));
            }
            catch (Exception e)
            {
                context.Exception = e;
                throw;
            }
            finally
            {
                if (context.Exception is null
                 || (context.Exception is not null
                  && context.ExecutePostEvent))
                    await OnPostAsync(context);
            }

            return context.GetResultValue<TResult>();
        }
        catch (Exception e)
        {
            context.Exception = e;
            await OnExceptionAsync(context);

            if (!context.SuppressException)
                throw;

            return default!;
        }
        finally
        {
            await OnExitAsync(context);
        }
    }

    /// <summary>
    /// Run before the process is invoked.
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnPre(InterceptorContext context)
    { }

    protected virtual Task OnPreAsync(InterceptorContext context) => Task.CompletedTask;

    /// <summary>
    /// Run after the process is invoked.
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnPost(InterceptorContext context)
    { }

    protected virtual Task OnPostAsync(InterceptorContext context) => Task.CompletedTask;

    /// <summary>
    /// Run in case of an exception
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnException(InterceptorContext context)
    { }

    protected virtual Task OnExceptionAsync(InterceptorContext context) => Task.CompletedTask;

    /// <summary>
    /// Run in final stage of the process.
    /// </summary>
    /// <param name="context"></param>
    protected virtual void OnExit(InterceptorContext context)
    { }

    protected virtual Task OnExitAsync(InterceptorContext context) => Task.CompletedTask;

    /// <summary>
    /// Check if the method can process the interceptor.
    /// </summary>
    /// <param name="invocation"></param>
    /// <returns></returns>
    private static bool CanProcess(IInvocation invocation)
    {
        // Get class interception attributes
        var classAttributes = invocation.InvocationTarget.GetType()
           .GetCustomAttributes(typeof(InterceptorAttribute<TInterceptor>), false)
           .ToHashSet();

        // Check if class does not have any interception attributes
        if (0 == classAttributes.Count)
            return false;

        // Get  current method interception attributes
        var methodAttributes = invocation.MethodInvocationTarget
           .GetCustomAttributes(typeof(InterceptorBaseAttribute), false)
           .ToHashSet();

        // Check if method attributes specify a NoIntercept attribute
        if (methodAttributes.Any(a => a is NoInterceptAttribute<TInterceptor> or NoInterceptAttribute))
            return false;

        // Check if method attributes specify an Intercept attribute
        if (methodAttributes.Any(a => a is InterceptAttribute))
            return true;

        // Check if the method has any interception attributes that are explicit,
        // if so, then return check if they match the current interceptor
        return !classAttributes.Cast<InterceptorAttribute<TInterceptor>>()
           .Any(
                classAttribute =>
                    classAttribute.Explicit
                 && !methodAttributes.Any(a => a is InterceptAttribute<TInterceptor>));
    }

    protected class InterceptorContext
    {
        public IInvocation? Invocation { get; init; }

        public Exception? Exception { get; set; }

        public IDictionary<string, object> Tags { get; } = new ConcurrentDictionary<string, object>();

        public dynamic? ResultValue { get; private set; }

        internal bool ExecutePostEvent { get; set; }

        internal bool SuppressException { get; set; }

        internal bool FlowBreak { get; set; }

        public void SetResultValue<T>(T value) => ResultValue = value;

        public T GetResultValue<T>() => Convert.ChangeType(ResultValue, typeof(T))!;

        public void ExecutePostEvenAfterFailure() => ExecutePostEvent = true;

        public void SuppressExceptions() => SuppressException = true;

        public void EnableFlowBreak() => FlowBreak = true;
    }
}

