using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroURLCore {
    internal abstract class ShortIdGenerator {
        protected int DesiredLength;
        public ShortIdGenerator(int desiredLength) {
            DesiredLength = desiredLength;
        }

        public abstract string GenerateShortId(string originalUrl);
    }
}
