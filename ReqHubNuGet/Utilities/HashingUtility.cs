using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReqHub
{
    public static class HashingUtility
    {
        private static readonly RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

        // Following https://apifriends.com/api-security/api-keys/
        public static (string token, string timestamp, string nonce) Create(string publicKey, string privateKey, string timestamp = null, string nonce = null)
        {
            if (timestamp == null)
            {
                // Get the UNIX timestamp
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
            }

            if (nonce == null)
            {
                // Create a nonce
                nonce = GenerateKey(size: 20);
            }

            // Concatenate all that with the private key for the secret
            var secret = $"{privateKey}{timestamp}{nonce}";

            // Compute the hash
            var token = Hash(publicKey, secret);
            return (token, timestamp, nonce);
        }

        public static string GenerateKey(int size = 32)
        {
            // From https://stackoverflow.com/questions/14843849/thread-safe-random-number-string-generator-for-an-oauth-nonce-in-c-sharp
            // and https://stackoverflow.com/questions/14412132/best-approach-for-generating-api-key
            // and https://security.stackexchange.com/questions/180345/do-i-need-to-hash-or-encrypt-api-keys-before-storing-them-in-a-database
            var buffer = new byte[size];
            random.GetNonZeroBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        public static string Hash(string value, string secret)
        {
            var secretBytes = Encoding.UTF8.GetBytes(secret);
            using (var hmacsha256 = new HMACSHA256(secretBytes))
            {
                hmacsha256.ComputeHash(Encoding.UTF8.GetBytes(value));
                var hash = Convert.ToBase64String(hmacsha256.Hash);
                return hash;
            }
        }
    }
}
