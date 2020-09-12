namespace Aviant.DDD.Application.Services
{
    #region

    using System.Collections.Generic;

    #endregion

    public interface ICsvFileBuilder<in TRecord>
        where TRecord : class
    {
        byte[] BuildTodoItemsFile(IEnumerable<TRecord> records);
    }
}