namespace Aviant.DDD.Domain.Messages
{
    using System.Collections.Generic;

    public interface IMessages
    {
        void AddMessage(string notification);

        bool HasMessages();

        List<string> GetAll();

        void CleanMessages();
    }
}