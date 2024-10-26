﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Drawing;

namespace MicroURLCore {
    /// <summary>
    /// Generator which return part of the hash, and if repetedly ask for hash for the same url, 
    /// it will return substring of same hash, but moved by 1.
    /// </summary>
    internal class HashBasedGenerator : ShortIdGenerator {
        private string LongUrl;
        private string Hash;
        private int Start;
        public HashBasedGenerator(int desiredLength) : base(desiredLength) { }

        public override string GenerateShortId(string originalUrl) {
            string hash = originalUrl == LongUrl ? Hash : GetHash(originalUrl);
            Start++;
            return hash.Substring(Start, Math.Min(DesiredLength, hash.Length - Start));
        }

        private string GetHash(string originalUrl) {
            LongUrl = originalUrl;
            Start = -1;
            using (SHA256 sha256 = SHA256.Create()) {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(originalUrl));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes) {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
