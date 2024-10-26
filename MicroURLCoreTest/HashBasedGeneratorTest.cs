using MicroURLCore.ShortIdGenerators;

namespace MicroURLCoreTest {
    [TestClass]
    public class HashBasedGeneratorTest {
        [TestMethod]
        public void HashFromSameUrlTest() {
            ShortIdGenerator generatr = new HashBasedGenerator(7);
            string longUrl = "http://subdomain.domain.com/wiki/company/product/1234567";
            Assert.AreEqual("6fa131e", generatr.GenerateShortId(longUrl));
            Assert.AreEqual("fa131e8", generatr.GenerateShortId(longUrl));
            Assert.AreEqual("a131e8f", generatr.GenerateShortId(longUrl));
            Assert.AreEqual("131e8fe", generatr.GenerateShortId(longUrl));
            Assert.AreEqual("31e8fe2", generatr.GenerateShortId(longUrl));
        }
    }
}
