using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsMigemo.Tests
{
    [TestClass]
    public class RomajiProcessorTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("ろーまじ", RomajiProcessor.ConvertRomajiToHiragana("ro-maji"));
            Assert.AreEqual("あっち", RomajiProcessor.ConvertRomajiToHiragana("atti"));
            Assert.AreEqual("あっt", RomajiProcessor.ConvertRomajiToHiragana("att"));
            Assert.AreEqual("wっw", RomajiProcessor.ConvertRomajiToHiragana("www"));
            Assert.AreEqual("っk", RomajiProcessor.ConvertRomajiToHiragana("kk"));
            Assert.AreEqual("ん", RomajiProcessor.ConvertRomajiToHiragana("n"));
            Assert.AreEqual("けんさく", RomajiProcessor.ConvertRomajiToHiragana("kensaku"));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var kiku = RomajiProcessor.RomajiToHiraganaPredictively("kiku");
            Assert.AreEqual("きく", kiku.EstaglishedHiragana);
            Assert.AreEqual(1, kiku.PredictiveSuffixes.Count);
            Assert.IsTrue(kiku.PredictiveSuffixes.Contains(""));

            var ky = RomajiProcessor.RomajiToHiraganaPredictively("ky");
            Assert.AreEqual("", ky.EstaglishedHiragana);
            Assert.IsTrue(ky.PredictiveSuffixes.Contains("きゃ"));

            var kky = RomajiProcessor.RomajiToHiraganaPredictively("kky");
            Assert.AreEqual("っ", kky.EstaglishedHiragana);
            Assert.IsTrue(kky.PredictiveSuffixes.Contains("きゃ"));

            var n = RomajiProcessor.RomajiToHiraganaPredictively("n");
            Assert.AreEqual("", n.EstaglishedHiragana);
            Assert.IsTrue(n.PredictiveSuffixes.Contains("ん"));
            Assert.IsTrue(n.PredictiveSuffixes.Contains("な"));
            Assert.IsTrue(n.PredictiveSuffixes.Contains("にゃ"));
            Assert.IsFalse(n.PredictiveSuffixes.Contains("っ"));

            var denk = RomajiProcessor.RomajiToHiraganaPredictively("denk");
            Assert.AreEqual("でん", denk.EstaglishedHiragana);
            Assert.IsTrue(denk.PredictiveSuffixes.Contains("か"));
        }
    }
}
