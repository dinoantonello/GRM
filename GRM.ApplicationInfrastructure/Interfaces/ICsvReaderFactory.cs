using System.IO;
using CsvHelper;

namespace GRM.ApplicationInfrastructure
{
    public interface ICsvReaderFactory
    {
        CsvReader CreateCsvReader(StreamReader reader);
    }
}