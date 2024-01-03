using System.Diagnostics;
using Aviant.Core.Collections.Extensions;
using Aviant.Core.Extensions;
using JetBrains.Annotations;

namespace Aviant.Core.Utilities;

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
            throw new ArgumentNullException(parameterName);

        return value;
    }

    [ContractAnnotation("value:null => halt")]
    public static string NotNullOrEmpty(string value, [InvokerParameterName] string parameterName)
    {
        if (value.IsNullOrEmpty())
            throw new ArgumentException(parameterName);

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
