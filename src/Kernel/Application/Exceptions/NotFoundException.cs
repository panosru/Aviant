using System.Runtime.Serialization;

namespace Aviant.Application.Exceptions;

[Serializable]
public sealed class NotFoundException : ApplicationException
{
    public NotFoundException(string message)
        : base(message)
    { }

    public NotFoundException(string message, Exception innerInner)
        : base(message, innerInner)
    { }

    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.")
    { }

    private NotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}
