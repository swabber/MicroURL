using MicroURLData;

namespace MicroURLCore {
    public class MicroURLService : IDisposable {
        private MicroUrlServiceConfig Config;
        private User currentUser;
        private HashSet<char> allowedSymbols = new HashSet<char>() { '$', '-', '_', '.', '+', '!', '*', '’', '(', ')', ',', '.' };

        public MicroURLService(MicroUrlServiceConfig config) {
            Config = config;

            using (UnitOfWork unitOfWork = new(Config.Context)) {
                currentUser = unitOfWork.UsersRepository.Get(Config.User);
                if (currentUser == null) {
                    throw new Exception("User not found;");
                }
            }
        }

        public string CreateShortURL(string longUrl) {
            using (UnitOfWork unitOfWork = new(Config.Context)) {
                string shortId = Config.Generator.GenerateShortId(longUrl);
                while (unitOfWork.IsShortIdExist(shortId)) { // repeat until not find unused hash.
                    shortId = Config.Generator.GenerateShortId(longUrl);
                    // hopefully this loop would not run many times.
                    if (!IsValid(shortId))  // also can add limit on number of try
                        throw new Exception($"Can not generate short Url for {longUrl}");
                }
                ShortLink link = new ShortLink() {
                    ShortId = shortId,
                    User = currentUser,
                    OriginalUrl = longUrl
                };
                unitOfWork.ShortLinkRepository.Add(link);
                return Config.Domain + shortId;
            }
        }

        public bool TrySetShortURL(string longUrl, string customShortUrl) {
            string shortId = ShortUrlToId(customShortUrl);
            if (!IsValid(shortId))
                return false;
            using (UnitOfWork unitOfWork = new(Config.Context)) {
                if (unitOfWork.IsShortIdExist(shortId))
                    return false;
                ShortLink link = new ShortLink() {
                    ShortId = shortId,
                    User = currentUser,
                    OriginalUrl = longUrl
                };
                unitOfWork.ShortLinkRepository.Add(link);
                return unitOfWork.IsShortIdExist(shortId);
            }
        }

        public bool DeleteShortURL(string shortUrl) {
            string shortId = ShortUrlToId(shortUrl);
            using (UnitOfWork unitOfWork = new(Config.Context)) {
                if (!unitOfWork.TryGetShortLinkById(shortId, currentUser, out ShortLink shortLink))
                    return true;   // User can delete only its own short urls.

                unitOfWork.ShortLinkRepository.Remove(shortLink);
                return !unitOfWork.IsShortIdExist(shortId);
            }
        }

        public bool DeleteAllShortURLs(string longURL) {
            using (UnitOfWork unitOfWork = new(Config.Context)) {
                var links = unitOfWork.ShortLinkRepository.Find(l => l.User == currentUser && l.OriginalUrl == longURL);
                unitOfWork.ShortLinkRepository.RemoveRange(links);
                links = unitOfWork.ShortLinkRepository.Find(l => l.User == currentUser && l.OriginalUrl == longURL);
                return links.Any();
            }
        }

        public List<string> GetAllShortURLs(string longURL) {
            using (UnitOfWork unitOfWork = new(Config.Context)) {
                List<string> res = new List<string>();
                foreach (var link in unitOfWork.ShortLinkRepository.Find(l => l.User == currentUser && l.OriginalUrl == longURL)) {
                    res.Add(Config.Domain + link.ShortId);
                }
                return res;
            }
        }

        public string GetLongFromShortURL(string shortUrl) {
            string shortId = ShortUrlToId(shortUrl);
            using (UnitOfWork unitOfWork = new(Config.Context)) {
                ShortLink link = unitOfWork.ShortLinkRepository.Get(shortId);
                Config.Statistics.UrlUsed(shortUrl);
                Config.Statistics.UrlUsed(link.OriginalUrl);
                return link.OriginalUrl;
            }
        }

        public string GetStatistics(string url) { 
            return Config.Statistics.GetUrlUsage(url);
        }

        private string ShortUrlToId(string shortUrl) {
            string shortId = shortUrl.Replace(Config.Domain, "").Trim().Trim('/');
            return shortId;
        }

        public bool IsValid(string? shortId) {
            if (string.IsNullOrWhiteSpace(shortId) || shortId.Length < 3 || shortId.Length > 12)
                return false;
            foreach (char c in shortId) {
                if (!char.IsLetterOrDigit(c) && !allowedSymbols.Contains(c))
                    return false;
            }
            return true;
        }

        public void Dispose() {
            Config.Context.Dispose();
        }
    }
}
