using GRM.ApplicationInfrastructure;
using GRM.Domain;
using GRM.Repository;
using GRM.Tests.IntegrationTests.DataFileHelpers;
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
    public class DistributionPartnerContractsTests
    {
      
        string _distributionPartnerContractsFileName = "Data/DistributionPartnerContracts.data";       
        string _distributionPartneFilePath;
        DataFileEditor<DistributionPartnerRaw> fileEditor = new DataFileEditor<DistributionPartnerRaw>(String.Empty);
        ICollection<DistributionPartnerRaw> _expected;

        public DistributionPartnerContractsTests()
        {

        }

        [SetUp]
        public void SetUp()
        {
            _distributionPartneFilePath = Path.Combine(fileEditor.GetRootDirectory(), _distributionPartnerContractsFileName);
            fileEditor = new DataFileEditor<DistributionPartnerRaw>(_distributionPartneFilePath);

            _expected = fileEditor.GetItemsFromFile();
        }

        [TearDown]
        public void Cleanup()
        {
            File.Copy(_distributionPartneFilePath + @".template", _distributionPartneFilePath, true);
        }


        public void WriteExpectedValuesToFile()
        {
            fileEditor.WriteItemsToFile(_expected);
        }


        public List<DistributionPartner> Execute()
        {
            var repository = new DistributionPartnerRepository(_distributionPartnerContractsFileName, new CsvReaderFactory());

            return repository.GetDistributionPartners();
        }

        [Test]
        public void PartnerShouldBeReadFromFile()
        {
            _expected.First().Partner = "MyTestPartner";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual(_expected.First().Partner, actual.First().Partner);
        }

        [Test]
        public void UsageShouldBeReadFromFile()
        {
            _expected.First().Usage = "MyTestUsage";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual(_expected.First().Usage, actual.First().Usage);
        }

       
    }
}
