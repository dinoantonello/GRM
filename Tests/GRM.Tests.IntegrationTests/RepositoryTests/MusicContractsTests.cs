using GRM.ApplicationInfrastructure;
using GRM.Domain;
using GRM.Repository;
using GRM.Tests.IntegrationTests.DataFileHelpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GRM.Tests.IntegrationTests.RepositoryTests
{
    [TestFixture]
    public class MusicContractsTests
    {
        string _musicContractsFileName = "Data/MusicContracts.data";
        string _musicContractsFilePath;
        string _distributionPartnerContractsFileName = "Data/DistributionPartnerContracts.data";      
        DataFileEditor<MusicContractRaw> _fileEditor = new DataFileEditor<MusicContractRaw>(String.Empty);
        ICollection<MusicContractRaw> _expected;

        public MusicContractsTests()
        {
           
        }

        [SetUp]
        public void SetUp() {
            _musicContractsFilePath = Path.Combine(_fileEditor.GetRootDirectory(), _musicContractsFileName);           
            _fileEditor = new DataFileEditor<MusicContractRaw>(_musicContractsFilePath);
            _expected = _fileEditor.GetItemsFromFile();

            File.Copy(_musicContractsFilePath + @".template", _musicContractsFilePath, true);
        }

        [TearDown]
        public void Cleanup()
        {
            File.Copy(_musicContractsFilePath + @".template", _musicContractsFilePath, true);
        }


        public List<MusicContract> Execute()
        {
            var partnerRepository = new DistributionPartnerRepository(_distributionPartnerContractsFileName, new CsvReaderFactory());
            var repository = new MusicContractsRepository(_musicContractsFileName, partnerRepository, new CsvReaderFactory());

            return repository.GetMusicContracts();
        }

        public void WriteExpectedValuesToFile()
        {
            _fileEditor.WriteItemsToFile(_expected);
        }

        [Test]
        public void ArtistShouldBeReadFromFile()
        {                    
            _expected.First().Artist = "Jim Jones";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual(_expected.First().Artist, actual.First().Artist);
        }

        [Test]
        public void TitleShouldBeReadFromFile()
        {
            _expected.First().Title = "My New Title";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual(_expected.First().Title, actual.First().Title);
        }

        [Test]
        public void PartnerInUsageShouldBeTranslatedFromDistributionPartnerContractsFile()
        {
            _expected.First().Usages = "digital download";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual("ITunes", actual.First().Usages.First().Partner);            
        }
       

       [Test]
        public void UsageInUsagesShouldBeTranslatedFromDistributionPartnerContractsFile()
        {
            _expected.First().Usages = "digital download";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual("digital download", actual.First().Usages.First().Usage);            
        }

        [Test]
        public void StartDateShouldBeReadFromFile()
        {
            _expected.First().StartDate = "1st Feb 2012";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual(new DateTime(2012, 02, 01), actual.First().StartDate);
        }


        [TestCase("January", 1)]
        [TestCase("February", 2)]
        [TestCase("March", 3)]
        [TestCase("April", 4)]
        [TestCase("May", 5)]
        [TestCase("June", 6)]
        [TestCase("July", 7)]
        [TestCase("August", 8)]
        [TestCase("September", 9)]
        [TestCase("October", 10)]
        [TestCase("November", 11)]
        [TestCase("December", 12)]
        public void StartDateCanBeFullMonthName(string month, int monthNumber)
        {
            _expected.First().StartDate = "1st " + month + " 2012";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual(new DateTime(2012, monthNumber, 01), actual.First().StartDate);
        }

        [TestCase("Jan", 1)]
        [TestCase("Feb", 2)]
        [TestCase("Mar", 3)]
        [TestCase("Apr", 4)]
        [TestCase("May", 5)]
        [TestCase("Jun", 6)]
        [TestCase("Jul", 7)]
        [TestCase("Aug", 8)]
        [TestCase("Sep", 9)]
        [TestCase("Oct", 10)]
        [TestCase("Nov", 11)]
        [TestCase("Dec", 12)]
        public void StartDateCanBeAbbreviatedMonthName(string month, int monthNumber)
        {
            _expected.First().StartDate = "1st " + month + " 2012";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual(new DateTime(2012, monthNumber, 01), actual.First().StartDate);
        }

        [Test]
        public void EndDateShouldBeReadFromFile()
        {
            _expected.First().EndDate = "31st Dec 2012";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual(new DateTime(2012, 12, 31), actual.Last().EndDate);
        }

        [Test]
        public void EndDateCanBeMissing()
        {
            _expected.First().EndDate = String.Empty;
            WriteExpectedValuesToFile();


            var actual = Execute();

            Assert.AreEqual(0, actual.First().EndDate.Ticks);
        }

        [TestCase("January", 1)]
        [TestCase("February", 2)]
        [TestCase("March", 3)]
        [TestCase("April", 4)]
        [TestCase("May", 5)]
        [TestCase("June", 6)]
        [TestCase("July", 7)]
        [TestCase("August", 8)]
        [TestCase("September", 9)]
        [TestCase("October", 10)]
        [TestCase("November", 11)]
        [TestCase("December", 12)]
        public void EndDateCanBeFullMonthName(string month, int monthNumber)
        {
            _expected.First().EndDate = "1st " + month + " 2012";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual(new DateTime(2012, monthNumber, 01), actual.First().EndDate);
        }

        [TestCase("Jan", 1)]
        [TestCase("Feb", 2)]
        [TestCase("Mar", 3)]
        [TestCase("Apr", 4)]
        [TestCase("May", 5)]
        [TestCase("Jun", 6)]
        [TestCase("Jul", 7)]
        [TestCase("Aug", 8)]
        [TestCase("Sep", 9)]
        [TestCase("Oct", 10)]
        [TestCase("Nov", 11)]
        [TestCase("Dec", 12)]
        public void EndDateCanBeAbbreviatedMonthName(string month, int monthNumber)
        {
            _expected.First().EndDate = "1st " + month + " 2012";
            WriteExpectedValuesToFile();

            var actual = Execute();

            Assert.AreEqual(new DateTime(2012, monthNumber, 01), actual.First().EndDate);
        }
    }
}
