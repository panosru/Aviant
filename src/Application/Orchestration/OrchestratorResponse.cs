namespace Aviant.Foundation.Application.Orchestration;

using System.Collections.ObjectModel;
using Ardalis.GuardClauses;

public sealed class OrchestratorResponse
{
    private readonly object? _payload;

    public OrchestratorResponse()
    { }

    public OrchestratorResponse(object? payload)
    {
        _payload  = payload;
        Succeeded = true;
    }

    public OrchestratorResponse(object? payload, int? affectedRows)
        : this(payload) => AffectedRows = affectedRows;

    public OrchestratorResponse(Collection<string> messages)
    {
        Messages  = messages;
        Succeeded = false;
    }

    public bool Succeeded { get; }

    public Collection<string> Messages { get; } = new();

    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private int? AffectedRows { get; }

    public object? Payload() => _payload;

    public T Payload<T>()
    {
        Guard.Against.Null(_payload, nameof(_payload));

        if (_payload is not T payload)
            throw new NotSupportedException(
                $@"Type ""{typeof(T).FullName}"" does not match payload type ""{_payload.GetType().FullName}""");

        return payload;
    }
}
