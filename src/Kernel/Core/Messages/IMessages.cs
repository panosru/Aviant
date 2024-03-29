namespace Aviant.Core.Messages;

public interface IMessages
{
    public void AddMessage(string message);

    public bool HasMessages();

    public List<string> GetAll();

    public void CleanMessages();
}
