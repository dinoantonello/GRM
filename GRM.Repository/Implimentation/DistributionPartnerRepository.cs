using GRM.ApplicationInfrastructure;
using GRM.Domain;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace GRM.Repository
{
    public class DistributionPartnerRepository : IDistributionPartnerRepository
    {

        string _filePath;
        ICsvReaderFactory _csvReaderFactory;

        public DistributionPartnerRepository(string distributionPartnerContractsFileName, ICsvReaderFactory csvReaderFactory)
        {
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);           
            _filePath = Path.Combine(executableLocation, distributionPartnerContractsFileName);
            _csvReaderFactory = csvReaderFactory;
        }

        public List<DistributionPartner> GetDistributionPartners()
        {   
            var partners = new List<DistributionPartner>();

            using (var reader = File.OpenText(_filePath))
            {
                var fileContents = _csvReaderFactory.CreateCsvReader(reader);

                while (fileContents.Read())
                {
                    var partner = new DistributionPartner();
                    partner.Partner = fileContents.GetField<string>(0);
                    partner.Usage = fileContents.GetField<string>(1);
                    partners.Add(partner);
                }
            }

            return partners;
        }
    }
}
