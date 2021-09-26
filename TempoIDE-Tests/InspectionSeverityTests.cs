using NUnit.Framework;
using TempoIDE.Core.Inspections;
using TempoIDE.Core.Inspections.Inspectors;

namespace TempoIDE_Tests
{
    [TestFixture]
    public class CsvTests
    {
        [Test]
        public void TestCsvCode()
        {
            var severity = InspectionSeverityAssociator.FromCode("JAMMO_0001");
            
            Assert.True(severity == InspectionSeverity.Hint);
        }
        
        [Test]
        public void TestInvalidCsvCode()
        {
            var severity = InspectionSeverityAssociator.FromCode("JAMMO_null");
            
            Assert.True(severity == InspectionSeverity.None);
        }
    }
}