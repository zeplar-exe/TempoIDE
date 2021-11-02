using System;
using System.Linq;
using NUnit.Framework;
using TempoPlugins;
using TempoPlugins.Internal.Parser;
using TempoPlugins.Internal.Syntax.Nodes;

namespace TempoPlugins_Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void TestDefine()
        {
            const string testString = "define x";
            var tree = new TelParser(testString).Parse();
            var syntax = (DefineSyntax)tree.Root.Nodes.First();
            
            Assert.True(syntax.Identifier.Name == "x");
        }

        [Test]
        public void TestDefineAsString()
        {
            const string testString = "define x as \"string\"";
            var tree = new TelParser(testString).Parse();
            var syntax = (DefineSyntax)tree.Root.Nodes.First();
            
            Assert.True(syntax.Initializer.Value.ToString() == "\"string\"");
        }
        
        [Test]
        public void TestDefineAsDecimal()
        {
            const string testString = "define x as 12.34 define x as 56.78";
            var tree = new TelParser(testString).Parse();
            var syntax = (DefineSyntax)tree.Root.Nodes.First();
            
            Assert.True(syntax.Initializer.Value.ToString() == "12.34");
        }
    }
}