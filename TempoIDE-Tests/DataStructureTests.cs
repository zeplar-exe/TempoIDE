using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TempoIDE.Core.DataStructures;

namespace TempoIDE_Tests
{
    [TestFixture]
    public class DataStructureTests
    {
        [TestFixture]
        public class BitTreeTests
        {
            [Test]
            public void TestFromTree()
            {
                var root = new MockItem(true);
                var nested1 = new MockItem(false);
                var nested2 = new MockItem(false);
                
                root.Items.Add(nested1);
                root.Items.Add(nested2);

                var tree = new MockTree(root);
                var bitTree = BitTree.FromTree(tree.Item, i => i.Items, i => i.Flag);
                
                Assert.True(bitTree.EnumerateTree().Count() == 3);
            }

            private class MockTree
            {
                public MockItem Item;

                public MockTree(MockItem item)
                {
                    Item = item;
                }
            }

            private class MockItem
            {
                public readonly List<MockItem> Items = new();
                public readonly bool Flag;

                public MockItem(bool flag)
                {
                    Flag = flag;
                }
            }
        }
    }
}