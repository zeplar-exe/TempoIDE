using System.Linq;
using NUnit.Framework;
using TempoPlugins;
using TempoPlugins.Syntax.Nodes;
using TempoPlugins.Syntax.Nodes.Expressions;

namespace TempoPlugins_Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        public void TestDefine()
        {
            const string testString = "define x";
            var tree = TelParser.Parse(testString);
            var syntax = (DefineSyntax)tree.Root.Nodes.First();
            
            Assert.True(syntax.Identifier == "x");
        }

        [Test]
        public void TestDefineAs()
        {
            const string testString = "define x as \"string\"";
            var tree = TelParser.Parse(testString);
            var syntax = (DefineSyntax)tree.Root.Nodes.First();
            var stringExpression = (StringExpressionSyntax)syntax.Assignment.Value;
            
            Assert.True(stringExpression.Literal.ToString() == "\"string\"");
        }
    }
}