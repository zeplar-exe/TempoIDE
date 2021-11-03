using System;
using System.IO;
using System.IO.Abstractions;
using NUnit.Framework;
using TempoIDE.Core.SettingsConfig;
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
            
            var appDirectory = root.CreateDirectory(@"Settings\app");
            appDirectory.CreateFile("skin.txt");
            
            root.CreateDirectory(@"Settings\editor");
            root.CreateDirectory(@"Settings\explorer");
            
            var directory = new SettingsDirectory(settingsDirectory.Info);
            
            directory.Parse();
            
            Assert.True(directory.AppSettings.SkinSettings.SkinConfig.CurrentSkin == "_default");
        }
        
        [Test]
        public void TestSkinConfig()
        {
            using var testString = "previous_skin=\"Light\" current_skin=\"Dark\"".CreateStream();
            var config = new SkinConfig(testString);
            
            config.Parse();
            
            Assert.True(config.CurrentSkin == "Dark");
        }
    }
}