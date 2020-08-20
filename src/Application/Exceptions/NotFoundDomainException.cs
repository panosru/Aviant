namespace Aviant.DDD.Application.Exceptions
{
    using System;

    public class NotFoundDomainException : ApplicationDomainException
    {
        public NotFoundDomainException(string message)
            :
            base(message)
        {
        }

        public NotFoundDomainException(string message, Exception innerException)
            :
            base(message, innerException)
        {
        }

        public NotFoundDomainException(string name, object key)
            :
            base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}