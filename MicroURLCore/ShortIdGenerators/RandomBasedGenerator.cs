
namespace MicroURLCore.ShortIdGenerators {
    /// <summary>
    /// Simpliest of all generators. At every request it will generate new random ShortID
    /// </summary>
    public class RandomBasedGenerator : ShortIdGenerator {
        private string AllowedSymbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789$-_.+!*’(),.";
        private Random Random = new();
        public RandomBasedGenerator(int desiredLength) : base(desiredLength) { }

        public override string GenerateShortId(string originalUrl = "") {
            char[] res = new char[DesiredLength];
            for (int i = 0; i < DesiredLength; i++) {
                int s = Random.Next(0, AllowedSymbols.Length);
                res[i] = AllowedSymbols[s];
            }
            return string.Concat(res);
        }
    }
}
