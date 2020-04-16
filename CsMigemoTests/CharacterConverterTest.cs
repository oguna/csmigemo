using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsMigemo.Tests
{
    [TestClass]
    public class CharacterConverterTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("ÉA", CharacterConverter.ConvertHira2Kata("Ç†"));
            Assert.AreEqual("ÉA", CharacterConverter.ConvertHan2Zen("±"));
            Assert.AreEqual("±", CharacterConverter.ConvertZen2Han("ÉA"));
        }
    }
}
