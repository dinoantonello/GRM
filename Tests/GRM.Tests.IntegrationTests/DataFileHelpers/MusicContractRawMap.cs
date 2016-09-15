using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM.Tests.IntegrationTests.DataFileHelpers
{
    public sealed class MusicContractRawMap : CsvClassMap<MusicContractRaw>
    {
        public MusicContractRawMap()
        {
            Map(m => m.Artist).Name("Artist");
            Map(m => m.Title).Name("Title");
            Map(m => m.Usages).Name("Usages");
            Map(m => m.StartDate).Name("StartDate");
            Map(m => m.StartDate).Name("EndDate");
        }
    }
    
}
