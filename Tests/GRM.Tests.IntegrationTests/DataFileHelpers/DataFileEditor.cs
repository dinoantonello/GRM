using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM.Tests.IntegrationTests.DataFileHelpers
{
    public class DataFileEditor<T> 
    {
        string _filePath;

        public DataFileEditor(string filePath)
        {
            _filePath = filePath;
        }

        public ICollection<T> GetItemsFromFile()
        {
            ICollection<T> itemsList;

            using (var reader = new StreamReader(_filePath))
            {
                var file = new CsvReader(reader);
                file.Configuration.Delimiter = "|";
                file.Configuration.HasHeaderRecord = true;
                itemsList = file.GetRecords<T>().ToList();
            }

            return itemsList;
        }

        public void WriteItemsToFile(ICollection<T> items)
        {   
            using (var sw = new StreamWriter(_filePath))
            {
                var writer = new CsvWriter(sw);
                writer.Configuration.HasHeaderRecord = true;
                writer.Configuration.Delimiter = "|";
                writer.WriteHeader<T>();
                writer.WriteRecords(items);
            }
        }

        public string GetRootDirectory()
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            var rootFilePathBuilder = new StringBuilder(string.Empty);
            rootFilePathBuilder.Append(System.IO.Path.GetDirectoryName(currentAssembly));
            rootFilePathBuilder.Replace("file:\\", string.Empty);

            rootFilePathBuilder.Append(@"\");
            return rootFilePathBuilder.ToString();
        }
    }
}
