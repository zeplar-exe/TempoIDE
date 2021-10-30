using System.Linq;
using NUnit.Framework;
using TempoIDE.Core.SettingsConfig.Internal.Parser;
using TempoIDE.Core.SettingsConfig.Settings;

namespace TempoIDE_Tests.SettingsTests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void TestTextSetting()
        {
            var testString = "thing=\"Hello world!\"";
            var setting = new SettingsParser(testString).ParseSettings().First();
            
            Assert.True(setting.Key == "thing" && setting.Value.ToString() == "Hello world!");
        }

        [Test]
        public void TestNumericTextSetting()
        {
            var testString = "thing=12.34";
            var setting = new SettingsParser(testString).ParseSettings().First();
            
            Assert.True(setting.Key == "thing" && setting.Value.ToString() == "12.34");
        }
        
        [Test]
        public void TestComment()
        {
            var testString = "thing=123 # This is a comment";
            var setting = new SettingsParser(testString).ParseSettingsAndComments().ElementAt(1);
            
            Assert.True(setting is Comment { Text: " This is a comment" });
        }
    }
}