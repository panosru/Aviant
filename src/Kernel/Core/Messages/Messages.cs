namespace Aviant.Core.Messages;

public sealed class Messages : IMessages
{
    private List<string> _messages = new();

    #region IMessages Members

    public void AddMessage(string message)
    {
        _messages.Add(message);
    }

    public void CleanMessages()
    {
        _messages = new List<string>();
    }

    public List<string> GetAll() => _messages;

    public bool HasMessages() => 0 < _messages.Count;

    #endregion
}
