
namespace MicroURLCore {
    public interface IStatistics {
        void UrlUsed(string url);
        string GetUrlUsage(string url);
    }
}
