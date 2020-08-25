namespace Aviant.DDD.Domain.Messages
{
    using System.Collections.Generic;

    public class Messages : IMessages
    {
        private List<string> _notifications = new List<string>();

        public void AddMessage(string notification)
        {
            _notifications.Add(notification);
        }

        public void CleanMessages()
        {
            _notifications = new List<string>();
        }

        public List<string> GetAll()
        {
            return _notifications;
        }

        public bool HasMessages()
        {
            return 0 < _notifications.Count;
        }
    }
}