using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroURLData {
    public class UnitOfWork : IUnitOfWork {

        private readonly MyBbContext Context;
        public UnitOfWork(MyBbContext context) {
            Context = context;
            UsersRepository = new UserRepository(context);
            ShortLinkRepository = new ShortLinkRepository (context);
        }

        public IUsersRepository UsersRepository { get; private set; }
        public IShortLinkRepository ShortLinkRepository { get; private set; }

        public int Complete() {
            return Context.SaveChanges();
        }

        public void Dispose() {
            // Context.Dispose();  // I'm not disposing context here. Because I saved nothing into persistence layer.
        }
    }
}
