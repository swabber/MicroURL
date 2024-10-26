using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroURLData {
    public class MyBbContext : IDisposable {
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
