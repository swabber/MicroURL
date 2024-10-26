
namespace MicroURLCore.ShortIdGenerators {
    /// <summary>
    /// This is counter based generator, where initial counter can be set by external sertice.
    /// </summary>
    public class CounterBasedGenerator : ShortIdGenerator {

        public CounterBasedGenerator(int desiredLength) : base(desiredLength) { }

        public override string GenerateShortId(string originalUrl) {
            throw new NotImplementedException(); // Intentionally not implemented due to lack of time.
        }
    }
}
