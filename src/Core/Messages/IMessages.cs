namespace Aviant.DDD.Core.Messages
{
    using System.Collections.Generic;

    public interface IMessages
    {
        void AddMessage(string message);

        bool HasMessages();

        List<string> GetAll();

        void CleanMessages();
    }
}