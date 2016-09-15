using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM.Tests.IntegrationTests.DataFileHelpers
{
    public sealed class DistributionPartnerRawMap : CsvClassMap<DistributionPartnerRaw>
    {
        public DistributionPartnerRawMap()
        {
            Map(m => m.Partner).Index(1);
            Map(m => m.Usage).Index(2);
        }
    }
    
}
