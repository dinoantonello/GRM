using GRM.ApplicationInfrastructure;
using GRM.Domain;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GRM.Repository
{
    public class MusicContractsRepository : IMusicContractsRepository
    {
        string _musicContractsFilePath;
        IDistributionPartnerRepository _distributionPartnerRepository;
        ICsvReaderFactory _csvReaderFactory;

        public MusicContractsRepository(string musicContractsFileName, IDistributionPartnerRepository distributionPartnerRepository, ICsvReaderFactory csvReaderFactory)
        {
            string executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _musicContractsFilePath = Path.Combine(executableLocation, musicContractsFileName);
            _distributionPartnerRepository = distributionPartnerRepository;
            _csvReaderFactory = csvReaderFactory;
        }

        public List<MusicContract> GetMusicContracts()
        {    
            var contracts = new List<MusicContract>();

            using (var reader = File.OpenText(_musicContractsFilePath))
            {
                var fileContents = _csvReaderFactory.CreateCsvReader(reader); 

                while (fileContents.Read())
                {
                    var contract = new MusicContract();
                    contract.Artist = fileContents.GetField<string>(0);
                    contract.Usages = AddUsages(fileContents.GetField<string>(2));
                    contract.Title = fileContents.GetField<string>(1);
                    contract.StartDate = fileContents.GetField<string>(3).ConvertToDateTime();
                    contract.EndDate = fileContents.GetField<string>(4).ConvertToDateTime();                   
                    contracts.Add(contract);
                }
            }

            return contracts;
        }

        private List<DistributionPartner> AddUsages(string usagesString)
        {
            var allPartners = _distributionPartnerRepository.GetDistributionPartners();
            var usagesStringArray = usagesString.Split(',');

            var partners = from distributionPartner in allPartners
                           where usagesStringArray.Any(usage => usage.Trim().ToLower() == distributionPartner.Usage.Trim().ToLower())
                           select distributionPartner;
             
            return partners.ToList();
        }
    }
}
