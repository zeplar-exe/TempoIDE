using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using TempoIDE.Core.SettingsConfig.Internal.Parser;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE_Tests
{
    [TestFixture]
    public class SettingsTests
    {
        [Test]
        public void TestComment()
        {
            var testString = "thing=123 # This is a comment";
            var setting = new SettingsParser(testString).Parse().ElementAt(1);
            
            Assert.True(setting.Value.ToString() == " This is a comment");
        }

        [Test]
        public void TestSkinConfig()
        {
            // TODO
        }
        
        [Test]
        public void TestTextSetting()
        {
            var testString = "thing=\"Hello world!\"";
            var setting = new SettingsParser(testString).Parse().First();
            
            Assert.True(setting.Key == "thing" && setting.Value.ToString() == "Hello world!");
        }

        [Test]
        public void TestNumericTextSetting()
        {
            var testString = "thing=12.34";
            var setting = new SettingsParser(testString).Parse().First();
            
            Assert.True(setting.Key == "thing" && setting.Value.ToString() == "12.34");
        }

        [Test]
        public void TestTruthyHelper()
        {
            Assert.True(true.IsTruthy() && 888.IsTruthy());
        }

        [Test]
        public void TestFalsy()
        {
            Assert.False("".IsTruthy() || new int[] { }.IsTruthy());
        }
    }
}