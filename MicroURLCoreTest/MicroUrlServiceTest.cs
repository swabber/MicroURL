using MicroURLCore;
using MicroURLCore.ShortIdGenerators;
using MicroURLData;

namespace MicroURLCoreTest {
    [TestClass]
    public class MicroUrlServiceTest {
        [TestMethod]
        public void LongToShortAndBackTest() {
            MicroUrlServiceConfig config = new("https://hire.me/", "User1", new DbContext(), new HashBasedGenerator(7), new ExternalStatisticsService());
            MicroURLService service = new MicroURLService(config);
            string longUrl = "http://subdomain.domain.com/wiki/company/product/1234567";
            string shortUrl = service.CreateShortURL(longUrl);

            string retrievedLong = service.GetLongFromShortURL(shortUrl);
            Assert.AreEqual(longUrl, retrievedLong);
        }

        [TestMethod]
        public void TrySetShortURLTest() {
            MicroUrlServiceConfig config = new("https://hire.me/", "User1", new DbContext(), new HashBasedGenerator(7), new ExternalStatisticsService());
            MicroURLService service = new MicroURLService(config);
            string longUrl = "http://subdomain.domain.com/wiki/company/product/1234567";
            string shortUrl = "https://hire.me/123";
            bool res = service.TrySetShortURL(longUrl, shortUrl);
            Assert.IsTrue(res);

            string retrievedLong = service.GetLongFromShortURL(shortUrl);
            Assert.AreEqual(longUrl, retrievedLong);
            service.DeleteShortURL(shortUrl);

            shortUrl = "https://hire.me/123/#$";  // Unallowed symbols used 
            res = service.TrySetShortURL(longUrl, shortUrl);
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void DeleteShortURLTest() {
            MicroUrlServiceConfig config = new("https://hire.me/", "User1", new DbContext(), new RandomBasedGenerator(7), new ExternalStatisticsService());
            MicroURLService service = new MicroURLService(config);
            string longUrl = "http://subdomain.domain.com/wiki/company/product/1234567";
            string shortUrl = service.CreateShortURL(longUrl);

            string retrievedLong = service.GetLongFromShortURL(shortUrl);
            Assert.AreEqual(longUrl, retrievedLong);

            bool res = service.DeleteShortURL(shortUrl);
            Assert.IsTrue(res);
            retrievedLong = service.GetLongFromShortURL(shortUrl);
            Assert.IsNull(retrievedLong);
        }

        [TestMethod]
        public void GetAllShortURLsTest() {
            MicroUrlServiceConfig config = new("https://hire.me/", "User1", new DbContext(), new RandomBasedGenerator(7), new ExternalStatisticsService());
            MicroURLService service = new MicroURLService(config);
            string longUrl = "http://subdomain.domain.com/wiki/company/product/1234567";
            string shortUrl1 = service.CreateShortURL(longUrl);
            string shortUrl2 = service.CreateShortURL(longUrl);

            var shortUrls = service.GetAllShortURLs(longUrl);
            Assert.IsTrue(shortUrls.Count == 2);
            Assert.IsTrue(shortUrls.Contains(shortUrl1));
            Assert.IsTrue(shortUrls.Contains(shortUrl2));
            service.DeleteShortURL(shortUrl1);
            service.DeleteShortURL(shortUrl2);

            shortUrls = service.GetAllShortURLs(longUrl);
            Assert.IsTrue(shortUrls.Count == 0);
        }

        [TestMethod]
        public void DeleteAllShortURLsTest() {
            MicroUrlServiceConfig config = new("https://hire.me/", "User1", new DbContext(), new RandomBasedGenerator(7), new ExternalStatisticsService());
            MicroURLService service = new MicroURLService(config);
            string longUrl = "http://subdomain.domain.com/wiki/company/product/1234567";
            string shortUrl1 = service.CreateShortURL(longUrl);
            string shortUrl2 = service.CreateShortURL(longUrl);

            var shortUrls = service.GetAllShortURLs(longUrl);
            Assert.IsTrue(shortUrls.Count == 2);
            service.DeleteAllShortURLs(longUrl);
            shortUrls = service.GetAllShortURLs(longUrl);
            Assert.IsFalse(shortUrls.Any());
        }

        [TestMethod]
        public void GetStatisticsTest() {
            MicroUrlServiceConfig config = new("https://hire.me/", "User1", new DbContext(), new HashBasedGenerator(7), new ExternalStatisticsService());
            MicroURLService service = new MicroURLService(config);
            string longUrl = "http://subdomain.domain.com/wiki/company/product/1234567";
            string shortUrl1 = service.CreateShortURL(longUrl);
            string shortUrl2 = service.CreateShortURL(longUrl);

            service.GetLongFromShortURL(shortUrl1);
            service.GetLongFromShortURL(shortUrl1);
            service.GetLongFromShortURL(shortUrl1);

            service.GetLongFromShortURL(shortUrl2);
            service.GetLongFromShortURL(shortUrl2);

            Assert.AreEqual("5", service.GetStatistics(longUrl));
            Assert.AreEqual("3", service.GetStatistics(shortUrl1));
            Assert.AreEqual("2", service.GetStatistics(shortUrl2));
        }
    }
}