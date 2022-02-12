namespace Aviant.Foundation.Application.Exceptions;

using System.Runtime.Serialization;
using Core.Exceptions;

[Serializable]
public class ApplicationException : CoreException
{
    public ApplicationException()
    { }

    public ApplicationException(string message)
        : base(message)
    { }

    public ApplicationException(string message, Exception inner)
        : base(message, inner)
    { }

    public ApplicationException(
        string     message,
        int        errorCode,
        int?       familyCode = null,
        Exception? inner      = null)
        : base(
            message,
            errorCode,
            familyCode,
            inner)
    { }

    protected ApplicationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}
