namespace Aviant.DDD.Infrastructure.Services
{
    #region

    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using Application.Services;
    using CsvHelper;
    using CsvHelper.Configuration;

    #endregion

    public class CsvFileBuilder<TRecrod, TMap> : ICsvFileBuilder<TRecrod>
        where TRecrod : class
        where TMap : ClassMap<TRecrod>
    {
        #region ICsvFileBuilder<TRecrod> Members

        public byte[] BuildTodoItemsFile(IEnumerable<TRecrod> records)
        {
            using var memoryStream = new MemoryStream();

            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.Configuration.RegisterClassMap<TMap>();
                csvWriter.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }

        #endregion
    }
}