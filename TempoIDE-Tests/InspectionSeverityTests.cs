using NUnit.Framework;
using TempoIDE.Core.Inspection;

namespace TempoIDE_Tests
{
    [TestFixture]
    public class CsvTests
    {
        [Test]
        public void TestCsvCodes()
        {
            var severity = InspectionSeverityAssociator.FromCode("JAMMO_0001");
            
            Assert.True(severity == InspectionSeverity.Hint);
        }
    }
}