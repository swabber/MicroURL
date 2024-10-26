
namespace MicroURLCore {
    /// <summary>
    /// This can be either separate Table or another database or some other service to offload work from main application.
    /// Because we have to prioritise conversion short URLs into long.
    /// </summary>
    public class ExternalStatisticsService : IStatistics {
        private Dictionary<string, int> Count = new Dictionary<string, int>();

        public void UrlUsed(string url) {
            if (string.IsNullOrEmpty(url))
                return;
            if (!Count.TryAdd(url, 1))
                Count[url]++;
        }

        public string GetUrlUsage(string url) {
            return Count.TryGetValue(url, out var usage) ? usage.ToString() : "";
        }
    }
}
