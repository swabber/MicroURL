﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroURLCore {
    /// <summary>
    /// Simpliest of all generators. At every request it will generate new random ShortID
    /// </summary>
    internal class RandomBasedGenerator : ShortIdGenerator {
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