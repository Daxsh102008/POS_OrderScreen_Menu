using System;
using System.Security.Cryptography;

namespace RESTAU
{
    /// <summary>
    /// Salted PBKDF2 password hashing.
    ///
    /// Stored format (single string, fits the Users.password nchar(100) column):
    ///     PBKDF2$&lt;iterations&gt;$&lt;saltBase64&gt;$&lt;hashBase64&gt;
    ///
    /// Every password gets its own random 16-byte salt, so two identical
    /// passwords never produce the same stored value. Verification recomputes
    /// the hash and compares in constant time.
    /// </summary>
    public static class PasswordHasher
    {
        private const string Prefix = "PBKDF2";
        private const int Iterations = 100000;
        private const int SaltBytes = 16;
        private const int HashBytes = 32;

        /// <summary>Hashes a password for storage. Never store the original.</summary>
        public static string Hash(string password)
        {
            if (password == null) throw new ArgumentNullException("password");

            byte[] salt = new byte[SaltBytes];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = Derive(password, salt, Iterations);

            return Prefix + "$" + Iterations.ToString()
                 + "$" + Convert.ToBase64String(salt)
                 + "$" + Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Checks a password against a stored value. Returns false (never throws)
        /// for anything that isn't a well-formed hash, so a legacy or corrupted
        /// row can't crash the login screen.
        /// </summary>
        public static bool Verify(string password, string stored)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(stored)) return false;

            string[] parts = stored.Split('$');
            if (parts.Length != 4 || parts[0] != Prefix) return false;

            int iterations;
            if (!int.TryParse(parts[1], out iterations) || iterations <= 0) return false;

            byte[] salt;
            byte[] expected;
            try
            {
                salt = Convert.FromBase64String(parts[2]);
                expected = Convert.FromBase64String(parts[3]);
            }
            catch (FormatException)
            {
                return false;
            }

            byte[] actual = Derive(password, salt, iterations);
            return FixedTimeEquals(actual, expected);
        }

        /// <summary>True if the value already looks like a stored hash.</summary>
        public static bool IsHashed(string stored)
        {
            return !string.IsNullOrEmpty(stored)
                && stored.StartsWith(Prefix + "$", StringComparison.Ordinal);
        }

        private static byte[] Derive(string password, byte[] salt, int iterations)
        {
            using (Rfc2898DeriveBytes kdf =
                   new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return kdf.GetBytes(HashBytes);
            }
        }

        /// <summary>
        /// Compares without short-circuiting, so response time doesn't leak how
        /// many leading bytes matched.
        /// </summary>
        private static bool FixedTimeEquals(byte[] a, byte[] b)
        {
            if (a == null || b == null || a.Length != b.Length) return false;

            int difference = 0;
            for (int i = 0; i < a.Length; i++)
            {
                difference |= a[i] ^ b[i];
            }
            return difference == 0;
        }
    }
}
