using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroURLData {
    internal class UserRepository : Repository<User>, IUsersRepository {
        public UserRepository(MyBbContext context) : base(context) { }

        public override User Get(string id) {
            User? user = Context.FirstOrDefault(u => u.UserId == id);
            if (user == null) {
                user = new User() { UserId = "Test User", Email="test@test.com", CreationDate = new DateTime(2000, 12, 31)};
                Add(user);
            }
            return user;
        }
    }
}
