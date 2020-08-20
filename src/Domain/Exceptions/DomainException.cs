namespace Aviant.DDD.Domain.Exceptions
{
    using System;

    public class DomainException : Exception //TODO: Revisit exceptions
    {
        public DomainException(string errorMessage)
            : base(errorMessage, null)
        {
        }

        // public DomainException(string name, object key)
        // {
        // }

        public DomainException(string errorMessage, Exception exception)
            : base($"The following error occurred \"{errorMessage}\"", exception)
        {
        }
    }
}