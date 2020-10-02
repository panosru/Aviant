namespace Aviant.DDD.Infrastructure.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using Core.Exceptions;

    public class InfrastructureException : DomainException
    {
        public InfrastructureException()
        { }

        public InfrastructureException(string message)
            : base(message)
        { }

        public InfrastructureException(string message, Exception inner)
            : base(message, inner)
        { }

        public InfrastructureException(
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

        protected InfrastructureException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}