using MicroURLData;

namespace MicroURLCore {
    public class MicroURLService : IDisposable {
        private MyBbContext Context;
        private ShortIdGenerator Generator;
        public string CurrentUser { get; private set; }
        public string DomainName { get; private set; }
        public MicroURLService(string currentUser) {
            CurrentUser = currentUser;
            DomainName = "https://hire.me/";
            Generator = GetGenerator();
            Context = new();
        }

        public string CreateShortURL(string longUrl) {
            using (UnitOfWork unitOfWork = new(Context)) {
                var user = unitOfWork.UsersRepository.Get(CurrentUser);
                if (user == null) {
                    throw new Exception("User not found;");
                }

                string shortId = Generator.GenerateShortId(longUrl);
                ShortLink link = unitOfWork.ShortLinkRepository.Get(shortId);
                while (link.ShortId == shortId) {
                    shortId = Generator.GenerateShortId(longUrl);   // repeat until not find unused hash.
                    link = unitOfWork.ShortLinkRepository.Get(shortId);     // hopfuly this loop would not run many times.
                }
                link.ShortId = shortId;
                link.User = user;
                link.OriginalUrl = longUrl;
                unitOfWork.ShortLinkRepository.Add(link);
                return DomainName + shortId;
            }
        }
        public bool SetShortURL(string longUrl, string customShortUrl) {
            return true;
        }
        public bool DeleteShortURL(string shortUrl) {
            return true;
        }
        public bool DeleteAllShortURLs(string longURL) {
            return true;
        }
        public List<string> GetAllShortURLs(string longURL) {
            return new List<string>() { "lkjkllk", "ljlkjlkj", "lkjlkjkljkl" };
        }
        public string GetLongFromShortURL(string shortId) {
            using (UnitOfWork unitOfWork = new(Context)) {
                ShortLink link = unitOfWork.ShortLinkRepository.Get(shortId);
                return link.OriginalUrl;
            }
        }

        public void Dispose() {
            CurrentUser = "";
        }

        private ShortIdGenerator GetGenerator() {
            return new HashBasedGenerator(7);
            //return new RandomBasedGenerator(7);
            //return new CounterBasedGenerator(7);
        }
    }
}
