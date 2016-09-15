using CsvHelper;
using System.IO;

namespace GRM.ApplicationInfrastructure
{
    public class CsvReaderFactory : ICsvReaderFactory
    {
        const string delimiter = "|";

        public CsvReader CreateCsvReader(StreamReader reader)
        {
            var fileContents = new CsvReader(reader);
            fileContents.Configuration.Delimiter = delimiter;
            return fileContents;
        }
        
    }
}
