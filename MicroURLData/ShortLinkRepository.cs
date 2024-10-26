using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroURLData {
    internal class ShortLinkRepository : Repository<ShortLink>, IShortLinkRepository {
        public ShortLinkRepository(MyBbContext context) : base(context) { }

        public override ShortLink Get(string id) {
            ShortLink? user = Context.FirstOrDefault(u => u.ShortId == id);
            if (user == null) {
                user = new ShortLink();
                Add(user);
            }
            return user;
        } 
    }
}
