using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CsMigemo.Tests
{
    [TestClass]
    public class CharacterConverterTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("�A", CharacterConverter.ConvertHira2Kata("��"));
            Assert.AreEqual("�A", CharacterConverter.ConvertHan2Zen("�"));
            Assert.AreEqual("�", CharacterConverter.ConvertZen2Han("�A"));
        }
    }
}
