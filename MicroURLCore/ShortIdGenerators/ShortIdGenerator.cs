
namespace MicroURLCore.ShortIdGenerators {
    /// <summary>
    /// Base class intentionally made as abstract so that if we implement other versions of hash generation,
    /// we can use reflection to instantiate them all the same way and be sure they have same constructor.
    /// </summary>
    public abstract class ShortIdGenerator {
        protected int DesiredLength;
        public ShortIdGenerator(int desiredLength) {
            DesiredLength = desiredLength;
        }

        public abstract string GenerateShortId(string originalUrl);
    }
}
