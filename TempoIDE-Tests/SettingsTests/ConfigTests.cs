using System.IO;
using NUnit.Framework;
using TempoIDE.Core.SettingsConfig.Directories;
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
            
            var appDirectory = settingsDirectory.CreateDirectory(@"app");
            appDirectory.CreateFile("skin.txt");
            
            settingsDirectory.CreateDirectory(@"editor");
            settingsDirectory.CreateDirectory(@"explorer");

            var directory = new SettingsDirectory(settingsDirectory.Info);
            
            Assert.True(directory.AppSettings.SkinSettings.SkinConfig.CurrentSkin == "_default");
        }
        
        [Test]
        public void TestSkinConfig()
        {
            using var testString = "previous_skin=\"Light\" current_skin=\"Dark\"".CreateStream();
            var config = new SkinConfig(testString);
            
            Assert.True(config.CurrentSkin == "Dark");
        }
    }
}