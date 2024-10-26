
namespace MicroURLData {
    internal interface IUnitOfWork : IDisposable {
        IUsersRepository UsersRepository { get; }
        IShortLinkRepository ShortLinkRepository { get; }
        int Complete();
    }
}
