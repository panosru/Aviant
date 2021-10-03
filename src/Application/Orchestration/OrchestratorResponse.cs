namespace Aviant.DDD.Application.Orchestration
{
    using System;
    using System.Collections.Generic;
    using Ardalis.GuardClauses;

    public sealed class OrchestratorResponse
    {
        private readonly object? _payload;

        public OrchestratorResponse()
        { }

        internal OrchestratorResponse(object? payload)
        {
            _payload  = payload;
            Succeeded = true;
        }

        internal OrchestratorResponse(object? payload, int? affectedRows)
            : this(payload) => AffectedRows = affectedRows;

        internal OrchestratorResponse(List<string> messages)
        {
            Messages  = messages;
            Succeeded = false;
        }

        public bool Succeeded { get; set; }

        public List<string> Messages { get; set; } = new();

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
}
