namespace Aviant.Foundation.Infrastructure.Services;

using System.Globalization;
using Application.Services;
using CsvHelper;
using CsvHelper.Configuration;

public sealed class CsvFileBuilder<TRecord, TMap> : ICsvFileBuilder<TRecord>
    where TRecord : class
    where TMap : ClassMap<TRecord>
{
    #region ICsvFileBuilder<TRecrod> Members

    public byte[] BuildTodoItemsFile(IEnumerable<TRecord> records)
    {
        using MemoryStream memoryStream = new();

        using (StreamWriter streamWriter = new(memoryStream))
        {
            using CsvWriter csvWriter = new(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.Context.RegisterClassMap<TMap>();
            csvWriter.WriteRecords(records);
        }

        return memoryStream.ToArray();
    }

    #endregion
}
