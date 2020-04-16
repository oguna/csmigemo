using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CsMigemo.Tests
{
    [TestClass]
    public class LoudsTrieTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var words = new string[] { "baby", "bad", "bank", "box", "dad", "dance" };
            Array.Sort(words);
            var trie = LoudsTrie.Build(words, out var indexes);
            for (int i = 0; i < words.Length; i++)
            {
                Assert.IsTrue(indexes[i] == trie.Get(words[i]));
            }
            var actual = trie.Get("box");
            Assert.AreEqual(10, actual);
            CollectionAssert.AreEqual(new ulong[] { 1145789805 }, trie.BitVector.Words);
            Assert.AreEqual(32, trie.BitVector.SizeInBits);
            var expectedEdge = "  bdaoabdnxdnykce".ToCharArray();
            CollectionAssert.AreEqual(expectedEdge, trie.Edges);
        }
    }
}
