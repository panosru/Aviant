namespace Aviant.DDD.Application.Orchestration
{
    using System;
    using System.Collections.Generic;

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

        public List<string> Messages { get; set; } = new List<string>();

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private int? AffectedRows { get; }

        public object? Payload() => _payload;

        public T Payload<T>()
        {
            if (_payload is null)
                throw new NullReferenceException(nameof(_payload));

            if (typeof(T) != _payload.GetType())
                throw new ApplicationException(
                    $@"Type ""{typeof(T).FullName}"" does not much payload type ""{_payload
                       .GetType().FullName}""");

            return (T) _payload;
        }
    }
}