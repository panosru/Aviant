namespace Aviant.Foundation.Application.Services;

public interface ICsvFileBuilder<in TRecord>
    where TRecord : class
{
    public byte[] BuildTodoItemsFile(IEnumerable<TRecord> records);
}
