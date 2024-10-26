
namespace MicroURLData {
    public class UnitOfWork : IUnitOfWork {

        private readonly DbContext Context;
        public UnitOfWork(DbContext context) {
            Context = context;
            UsersRepository = new UserRepository(context);
            ShortLinkRepository = new ShortLinkRepository (context);
        }

        public IUsersRepository UsersRepository { get; private set; }
        public IShortLinkRepository ShortLinkRepository { get; private set; }

        public bool IsShortIdExist(string shortId) {
            return ShortLinkRepository.Find(l => l.ShortId == shortId).Any();
        }

        public bool TryGetShortLinkById(string shortId, User user, out ShortLink shortLink) {
            List<ShortLink> links = ShortLinkRepository.Find(l => l.User == user && l.ShortId == shortId).ToList();
            shortLink = null;
            if (links.Any()) {
                shortLink = links.First();
                return true;
            }
            return false;
        }

        public int Complete() {
            return Context.SaveChanges();
        }

        public void Dispose() {
            // Context.Dispose();  // I'm not disposing context here. Because I saved nothing into persistence layer.
        }
    }
}
