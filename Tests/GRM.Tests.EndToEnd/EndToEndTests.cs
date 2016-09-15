using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GRM.Tests.EndToEnd
{
    
    [TestFixture]
    class EndToEndTests
    {
        
        [TestCase("ITunes 1st March 2012", "4 Records Returned", Description = "HappyPathTestProvidedByClient1")]
        [TestCase("YouTube 1st April 2012", "2 Records Returned", Description = "HappyPathTestProvidedByClient2")]
        [TestCase("YouTube 27th Dec 2012", "4 Records Returned", Description = "HappyPathTestProvidedByClient3")]
        [TestCase("YouTube", "Unable to find a valid effective date", Description = "ShouldReturnErrorMessageIfEffectiveDateIsNotProvided")]
        [TestCase("", "Unable to find a valid effective date", Description = "ShouldReturnErrorMessageIfEffectiveDateIsNotProvided")]
        [TestCase("", "Unable to find a valid delivery partner name", Description = "ShouldReturnErrorMessageIfDeliveryParnerNameIsNotProvided")]
        public void EndToEndTest(string input, string expected)
        {
            
            using (var sw = new StringWriter())
            {
                using (var sr = new StringReader("200"))
                {
                    
                    Console.SetOut(sw);
                    Console.SetIn(sr);

                    GRM.UI.Console.Program.Main(input.Split(' '));
                    
                    var result = sw.ToString();

                    Assert.IsTrue(result.Contains(expected), result);
                }
            }
        }
    }
}

