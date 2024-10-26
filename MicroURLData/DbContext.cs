
namespace MicroURLData {
    public class DbContext : IDisposable {
        private Dictionary<Type, object> tables = new Dictionary<Type, object>();
        public object GetDefault(Type type, object defaultTable) {
            tables.TryAdd(type, defaultTable);
            return tables[type];
        }

        public int SaveChanges() {
            return 1;
        }

        public void Dispose() {
            tables.Clear();
        }
    }
}
