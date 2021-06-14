using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptExApi.Utilities
{
    public class StringUtilities
    {
        public const string AlphabetMin = "abcdefghijklmnopqrstuvwxyz";
        public const string AlphabetMaj = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string Numbers = "0123456789";
        public const string SpecialChars = @"+@*#%&/\|()=?^-_.,:;éèà$¨<>";
        public const string Space = " ";

        private static readonly Random random = new Random();

        /// <summary>
        /// Get a pseudo-random string.
        /// </summary>
        /// <param name="length">String length</param>
        /// <param name="allowedChars">Which characters to use.</param>
        /// <returns></returns>
        public static string Random(int length, AllowedChars allowedChars = AllowedChars.All)
        {
            var chars = string.Empty;

            if (allowedChars.HasFlag(AllowedChars.AlphabetMin))
                chars += AlphabetMin;
            if (allowedChars.HasFlag(AllowedChars.AlphabetMaj))
                chars += AlphabetMaj;
            if (allowedChars.HasFlag(AllowedChars.Numbers))
                chars += Numbers;
            if (allowedChars.HasFlag(AllowedChars.SpecialChars))
                chars += SpecialChars;
            if (allowedChars.HasFlag(AllowedChars.Spaces))
                chars += Space;

            var stringChars = new char[length];

            for (int i = 0; i < stringChars.Length; i++) {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }

        /// <summary>
        /// Get a secure random string.
        /// </summary>
        /// <param name="length">String length</param>
        /// <param name="allowedChars">Which characters to use.</param>
        /// <returns></returns>
        public static string SecureRandom(int length, AllowedChars allowedChars = AllowedChars.All)
        {
            var chars = string.Empty;

            if (allowedChars.HasFlag(AllowedChars.AlphabetMin))
                chars += AlphabetMin;
            if (allowedChars.HasFlag(AllowedChars.AlphabetMaj))
                chars += AlphabetMaj;
            if (allowedChars.HasFlag(AllowedChars.Numbers))
                chars += Numbers;
            if (allowedChars.HasFlag(AllowedChars.SpecialChars))
                chars += SpecialChars;
            if (allowedChars.HasFlag(AllowedChars.Spaces))
                chars += Space;

            var stringChars = new char[length];

            for (int i = 0; i < stringChars.Length; i++) {
                stringChars[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
            }

            return new string(stringChars);
        }

        /// <summary>
        /// Hash a string (default algorithm: SHA256)
        /// </summary>
        /// <param name="rawData">String to hash</param>
        /// <param name="hashAlgorithm">Hash algorithm to use (default: SHA256)</param>
        /// <returns>Computed hash</returns>
        public static string ComputeHash(string rawData, HashAlgorithmName? hashAlgorithm = null)
        {
            hashAlgorithm = hashAlgorithm.HasValue ? hashAlgorithm : HashAlgorithmName.SHA256;

            // Create a hash algorithm instance
            using (var hashAlgorithmInstance = HashAlgorithm.Create(hashAlgorithm.Value.Name)) {
                // ComputeHash - returns byte array
                byte[] bytes = hashAlgorithmInstance.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                var builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                    builder.Append(bytes[i].ToString("x2"));

                return builder.ToString();
            }
        }

        [Flags]
        public enum AllowedChars
        {
            AlphabetMin = 1,
            AlphabetMaj = 2,
            Numbers = 4,
            SpecialChars = 8,
            Spaces = 16,

            All = AlphabetMin | AlphabetMaj | Numbers | SpecialChars,
            AllSpaces = All | Spaces,
            Alphabet = AlphabetMin | AlphabetMaj,
            AlphabetNumbers = Alphabet | Numbers
        }
    }
}
