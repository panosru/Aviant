namespace Aviant.DDD.Application.Exceptions
{
    #region

    using System;

    #endregion

    public class NotFoundException : ApplicationException
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
    }
}