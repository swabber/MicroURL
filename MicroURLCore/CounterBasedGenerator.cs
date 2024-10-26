using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroURLCore {
    internal class CounterBasedGenerator : ShortIdGenerator {
        
        public CounterBasedGenerator(int desiredLength) : base(desiredLength) { }

        public override string GenerateShortId(string originalUrl) {
            throw new NotImplementedException();
        }
    }
}
