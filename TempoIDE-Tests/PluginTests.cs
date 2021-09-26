using NUnit.Framework;
using TempoIDE.Plugins;
using TempoIDE.Plugins.Core;

namespace TempoIDE_Tests
{
    public class PluginTests
    {
        private const string TestString = "Version 1 meta={SomeName:\"SomeValue\"}";
        private readonly PluginStream stream = PluginParser.Parse(TestString);

        [Test]
        public void TestVersion()
        {
            Assert.True(stream.Version == "1");
        }

        [Test]
        public void TestMetadata()
        {
            Assert.True(stream.Metadata["SomeName"] == "SomeValue");
        }
    }
}