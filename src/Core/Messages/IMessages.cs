namespace Aviant.DDD.Core.Messages
{
    #region

    using System.Collections.Generic;

    #endregion

    public interface IMessages
    {
        void AddMessage(string message);

        bool HasMessages();

        List<string> GetAll();

        void CleanMessages();
    }
}