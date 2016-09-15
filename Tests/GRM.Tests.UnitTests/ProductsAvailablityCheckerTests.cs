using GRM.ApplicationInfrastructure;
using GRM.Domain;
using GRM.Repository;
using GRM.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM.Tests.IntegrationTests.RepositoryTests
{
    [TestFixture]
    public class ProductsAvailablityCheckerTests
    {
        string _deliveryPartnerName;
        DateTime _effectiveDate;
        Mock<IMusicContractsRepository> _repository;
        List<MusicContract> sampleContracts;

        public ProductsAvailablityCheckerTests()
        {
           
        }

        private List<MusicContract> GetSampleContracts() {
            var digitalDownload = new DistributionPartner() { Partner = "ITunes", Usage = "digital download" };
            var streaming = new DistributionPartner() { Partner = "YouTube", Usage = "streaming" };
            var contracts = new List<MusicContract>();
            contracts.Add(new MusicContract() { Artist = "Tinie Tempah", Title = "Frisky(Live from SoHo)", Usages = new List<DistributionPartner>() { digitalDownload, streaming }, StartDate = new DateTime(2012, 2, 1) });
            contracts.Add(new MusicContract() { Artist = "Tinie Tempah", Title = "Miami 2 Ibiza", Usages = new List<DistributionPartner>() { digitalDownload }, StartDate = new DateTime(2012, 2, 1) });
            contracts.Add(new MusicContract() { Artist = "Tinie Tempah", Title = "Till I'm Gone", Usages = new List<DistributionPartner>() { digitalDownload }, StartDate = new DateTime(2012, 8, 1) });
            contracts.Add(new MusicContract() { Artist = "Monkey Claw", Title = "Black Mountain", Usages = new List<DistributionPartner>() { digitalDownload }, StartDate = new DateTime(2012, 2, 1) });
            contracts.Add(new MusicContract() { Artist = "Monkey Claw", Title = "Iron Horse", Usages = new List<DistributionPartner>() { digitalDownload, streaming }, StartDate = new DateTime(2012, 6, 1) });
            contracts.Add(new MusicContract() { Artist = "Monkey Claw", Title = "Motor Mouth", Usages = new List<DistributionPartner>() { digitalDownload, streaming }, StartDate = new DateTime(2011, 3, 1) });
            contracts.Add(new MusicContract() { Artist = "Monkey Claw", Title = "Motor Mouth", Usages = new List<DistributionPartner>() { streaming }, StartDate = new DateTime(2012, 12, 25), EndDate = new DateTime(2012, 12, 31) });
            return contracts;
        }
        
        private void SetAllEndDatesToAYearAhead()
        {
            foreach (var contract in sampleContracts)
            {                
                contract.EndDate = DateTime.Now.AddYears(1);
            }
        }
    
           
        private void SetAllStartDatesToAYearAgo()
        {
            foreach (var contract in sampleContracts)
            {
                contract.StartDate = DateTime.Now.AddYears(-1);                
            }
        }

        private int CountSampleContractsWithPartnerNameOf(string deliveryPartnerName)
        {
            return (
                        from con in GetSampleContracts()
                        where con.Usages.Any(y => y.Partner.Trim().ToLower() == deliveryPartnerName.Trim().ToLower())
                        select con
                    ).Count();
        }

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IMusicContractsRepository>();
            sampleContracts = GetSampleContracts();
        }                

        public IEnumerable<MusicContract> Execute()
        {
            _repository.Setup(x => x.GetMusicContracts()).Returns(sampleContracts);

            var service = new ContractAvailablityChecker(_repository.Object);

            return service.GetContracts(_deliveryPartnerName, _effectiveDate);
        }

        [Test]
        public void ShouldReturnContractsMatchingDeliveryPartnerName()
        {
            _deliveryPartnerName = "ITunes";
            SetAllStartDatesToAYearAgo();
            SetAllEndDatesToAYearAhead();
            _effectiveDate = DateTime.Now;


            var actual = Execute();


            var actualCount = actual.Count();
            int expectedCount = CountSampleContractsWithPartnerNameOf(_deliveryPartnerName);

            Assert.AreEqual(expectedCount, actualCount);
            Assert.AreNotEqual(actualCount, 0);
            Assert.AreNotEqual(expectedCount, 0);

        }




        [TestCase(-2, 0, Description = "ShouldNotReturnContractsThatHaveNotYetStarted") ]
        [TestCase(0, 7, Description = "ShouldReturnContractsThatAreCurrent")]
        [TestCase(2, 0, Description = "ShouldNotReturnContractsThatHaveExpired")]
        public void ShouldReturnContractsThatAreCurrentAsOfEffectiveDate(int yearsToAdd, int expectedCount)
        {
            // nullify delivery Partner Name as a criteria
            _deliveryPartnerName = "ITunes";
            SetAllDeliveryPartners(_deliveryPartnerName);

            // dates set up
            SetAllStartDatesToAYearAgo();
            SetAllEndDatesToAYearAhead();
            _effectiveDate = DateTime.Now.AddYears(yearsToAdd);

            var actual = Execute();


            var actualCount = actual.Count();

            Assert.AreEqual(expectedCount, actualCount);

        }

        [Test]
        public void ShouldReturnContractsOrderByArtist()
        {
            _deliveryPartnerName = "ITunes";
            _effectiveDate = new DateTime(2012, 2, 1);


            sampleContracts = new List<MusicContract>();
            var unorderedList = new List<string> { "D", "C", "B", "A" };
            foreach (var artist in unorderedList)
            {
                sampleContracts.Add(new MusicContract() { Artist = artist, Title = "A", Usages = new List<DistributionPartner>() { new DistributionPartner() { Partner = _deliveryPartnerName, Usage = "digital download" } }, StartDate = _effectiveDate.AddDays(-1) });
            }

            var actual = Execute();


            var orderedList = unorderedList.OrderBy(x => x).ToList();
            for (int i = 0; i < orderedList.Count; i++)
            {
                Assert.AreEqual(orderedList.ElementAt(i), actual.ElementAt(i).Artist);
            }


        }

        private void SetAllDeliveryPartners(string deliveryPartnerName)
        {
            foreach (var contract in sampleContracts)
            {
                contract.Usages = new List<DistributionPartner> { new DistributionPartner() { Partner = deliveryPartnerName } };
            }
        }
    }
}
