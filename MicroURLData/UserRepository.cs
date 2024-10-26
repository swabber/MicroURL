
namespace MicroURLData {
    internal class UserRepository : Repository<User>, IUsersRepository {
        public UserRepository(DbContext context) : base(context) { }
        
        public override User Get(string id) {
            User? user = Context.FirstOrDefault(u => u.UserId == id);
            if (user == null) { // Only for mock purposes.
                user = new User() { UserId = "Test User", Email="test@test.com", CreationDate = new DateTime(2000, 12, 31)};
                Add(user);
            }
            return user;
        }
    }
}
