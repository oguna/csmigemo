using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsMigemo.Tests
{
    [TestClass]
    public class CharacterConverterTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("ア", CharacterConverter.ConvertHira2Kata("あ"));
            Assert.AreEqual("ア", CharacterConverter.ConvertHan2Zen("ｱ"));
            Assert.AreEqual("ｱ", CharacterConverter.ConvertZen2Han("ア"));
        }
    }
}
