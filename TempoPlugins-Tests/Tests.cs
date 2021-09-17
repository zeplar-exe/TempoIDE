using System;
using System.Linq;
using NUnit.Framework;
using TempoPlugins;

namespace TempoPlugins_Tests
{
    public class Tests
    {
        [Test]
        public void TestVersion()
        {
            var testString = "Version 1 meta={SomeName:\"SomeValue\"}";
            var stream = PluginParser.Parse(testString);
            Console.WriteLine("--" + stream.Version + "--");
            Assert.True(stream.Version == "1");
        }

        [Test]
        public void TestMetadata()
        {
            var testString = "Version 1 meta={SomeName:\"SomeValue\"}";
            var stream = PluginParser.Parse(testString);
            
            Assert.True(stream.Metadata["SomeName"] == "SomeValue");
        }
    }
}