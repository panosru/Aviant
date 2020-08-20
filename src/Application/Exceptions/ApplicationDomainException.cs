namespace Aviant.DDD.Application.Exceptions
{
    using System;
    using Domain.Exceptions;

    public class ApplicationDomainException : DomainException //TODO: Revisit exceptions 
    {
        public ApplicationDomainException(string errorMessage)
            : base(errorMessage)
        {
        }

        // public ApplicationDomainException(string name, object key)
        //     : base(name, key)
        // {
        // }

        public ApplicationDomainException(string errorMessage, Exception exception)
            : base(errorMessage, exception)
        {
        }
    }
}