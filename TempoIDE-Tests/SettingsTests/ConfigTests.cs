using System.IO;
using System.IO.Abstractions;
using NUnit.Framework;
using TempoIDE.Core.SettingsConfig;
using TempoIDE.Core.SettingsConfig.Settings.SettingsFiles;

namespace TempoIDE_Tests.SettingsTests
{
    [TestFixture]
    public class ConfigTests
    {
        [Test]
        public void TestSettingsDirectory()
        {
            var fileSystem = new MockFileSystem("mock", new DirectoryInfo(Directory.GetCurrentDirectory()));
            var root = fileSystem.Root;

            var settingsDirectory = root.CreateDirectory("Settings");
            
            var appDirectory = root.CreateDirectory(@"Settings\app");
            appDirectory.CreateFile("skin.txt");
            
            root.CreateDirectory(@"Settings\editor");
            root.CreateDirectory(@"Settings\explorer");
            
            var directory = SettingsDirectory.Create(settingsDirectory.Info);
            
            Assert.True(directory.AppSettings.SkinConfig.CurrentSkin == "_default");
        }
        
        [Test]
        public void TestSkinConfig()
        {
            var testString = "previous_skin=\"Light\" current_skin=\"Dark\"";
            var config = new SkinConfig(testString.CreateStream());
            
            Assert.True(config.CurrentSkin == "Dark");
        }
    }
}