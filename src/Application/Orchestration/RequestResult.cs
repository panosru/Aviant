namespace Aviant.DDD.Application.Orchestration
{
    using System;
    using System.Collections.Generic;

    //TODO: Revisit

    public sealed class RequestResult
    {
        private readonly object? _payload;

        public RequestResult()
        { }

        internal RequestResult(object? payload)
        {
            _payload  = payload;
            Succeeded = true;
        }

        internal RequestResult(object? payload, int? affectedRows)
            : this(payload) => AffectedRows = affectedRows;

        internal RequestResult(List<string> messages)
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
                throw new Exception("Payload is null");

            if (typeof(T) != _payload.GetType())
                throw new Exception(
                    $@"Type ""{typeof(T).FullName}"" does not much payload type ""{_payload
                       .GetType().FullName}""");

            return (T) _payload;
        }
    }
}