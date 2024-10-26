using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroURLData {
    internal interface IUnitOfWork : IDisposable {
        IUsersRepository UsersRepository { get; }
        IShortLinkRepository ShortLinkRepository { get; }
        int Complete();
    }
}
