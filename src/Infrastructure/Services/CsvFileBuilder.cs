namespace Aviant.DDD.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
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
            using var memoryStream = new MemoryStream();

            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.Context.RegisterClassMap<TMap>();
                csvWriter.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }

        #endregion
    }
}