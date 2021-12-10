namespace Aviant.DDD.Core.Utilities;

using System.Diagnostics;
using Collections.Extensions;
using Extensions;
using JetBrains.Annotations;

[DebuggerStepThrough]
public static class Check
{
    [ContractAnnotation("value:null => halt")]
    public static T NotNull<T>(T value, [InvokerParameterName] string parameterName)
    {
        if (value is null)
            throw new ArgumentNullException(parameterName);

        return value;
    }

    [ContractAnnotation("value:null => halt")]
    public static string NotNullOrWhiteSpace(string value, [InvokerParameterName] string parameterName)
    {
        if (value.IsNullOrWhiteSpace())
            throw new ArgumentException($"{parameterName} can not be null, empty or white space!", parameterName);

        return value;
    }

    [ContractAnnotation("value:null => halt")]
    public static string NotNullOrEmpty(string value, [InvokerParameterName] string parameterName)
    {
        if (value.IsNullOrEmpty())
            throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);

        return value;
    }

    [ContractAnnotation("value:null => halt")]
    public static ICollection<T> NotNullOrEmpty<T>(
        ICollection<T>                value,
        [InvokerParameterName] string parameterName)
    {
        if (value.IsNullOrEmpty())
            throw new ArgumentException($"{parameterName} can not be null or empty!", parameterName);

        return value;
    }
}
