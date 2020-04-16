using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsMigemo.Tests
{
    [TestClass]
    public class RegexGeneratorTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var rg = new RegexGenerator();
            rg.Add("a");
            rg.Add("b");
            rg.Add("c");
            Assert.AreEqual("[abc]", rg.Generate(RegexOperator.DEFAULT));
        }

        [TestMethod]
        public void TestMethod2()
        {
            var rg = new RegexGenerator();
            Assert.AreEqual("", rg.Generate(RegexOperator.DEFAULT));
        }

        [TestMethod]
        public void TestMethod3()
        {
            var rg = new RegexGenerator();
            rg.Add("ab");
            rg.Add("ac");
            rg.Add("z");
            Assert.AreEqual("(z|a[bc])", rg.Generate(RegexOperator.DEFAULT));
        }
    }
}
