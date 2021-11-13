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
            
            Assert.True(setting.Key == "thing" && setting.Value.ToString().StartsWith("12.34"));
        }

        [Test]
        public void TestBooleanSetting()
        {
            var testString = "thing=TRUE";
            var setting = new SettingsParser(testString).ParseSettings().First();
            
            Assert.True(setting.Value is BooleanSetting { Value: true });
        }

        [Test]
        public void TestTreeSetting()
        {
            var testString = "thing=[ a=\"abc\" b=\"def\" ]";
            var setting = new SettingsParser(testString).ParseSettings().First();
            var value = (SettingTree)setting.Value;
            
            Assert.True(value.Settings.ElementAt(1).Value.ToString() == "def");
        }
    }
}