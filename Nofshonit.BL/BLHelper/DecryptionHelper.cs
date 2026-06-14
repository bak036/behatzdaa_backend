using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Nofshonit.BL.BLHelper
{
    /// <summary>
    /// Helper for decrypting the Apigee / IDF consumer_secret at runtime.
    /// The encrypted value is read from web.config / appsettings.json
    /// (key: BehatzdaaApigeeEncryptedSecret), the salt is read from dbo.AppConfig
    /// (key: BehatzdaaApigeeSecretSalt) and the encryption key is read from an
    /// environment variable (BEHATZDAA_APIGEE_ENCRYPTION_KEY).
    ///
    /// The AES key is derived at runtime using PBKDF2 (Rfc2898DeriveBytes.Pbkdf2),
    /// the IV is read from the beginning of the encrypted blob.
    ///
    /// SECURITY: never log the plain or decrypted consumer_secret, the encryption
    /// key, the derived AES key, the salt or the full ciphertext.
    /// </summary>
    public static class DecryptionHelper
    {
        // Must match the values used by the encryption console app.
        private const int KeySizeBytes = 32;
        private const int IvSizeBytes = 16;
        private const int Iterations = 100_000;

        /// <summary>
        /// Decrypts a Base64-encoded (IV + AES ciphertext) blob using a key derived
        /// from <paramref name="encryptionKey"/> and <paramref name="saltBase64"/>
        /// via PBKDF2-SHA256.
        /// </summary>
        public static string DecryptSecret(string encryptedSecretBase64, string encryptionKey, string saltBase64)
        {
            if (string.IsNullOrWhiteSpace(encryptedSecretBase64))
                throw new ArgumentException("encryptedSecretBase64 is required.", nameof(encryptedSecretBase64));

            if (string.IsNullOrWhiteSpace(encryptionKey))
                throw new ArgumentException("encryptionKey is required.", nameof(encryptionKey));

            if (string.IsNullOrWhiteSpace(saltBase64))
                throw new ArgumentException("saltBase64 is required.", nameof(saltBase64));

            byte[] encryptedBytesWithIv;
            byte[] saltBytes;

            try
            {
                encryptedBytesWithIv = Convert.FromBase64String(encryptedSecretBase64);
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("BehatzdaaApigeeEncryptedSecret is not a valid Base64 string.", ex);
            }

            try
            {
                saltBytes = Convert.FromBase64String(saltBase64);
            }
            catch (FormatException ex)
            {
                throw new InvalidOperationException("BehatzdaaApigeeSecretSalt is not a valid Base64 string.", ex);
            }

            if (encryptedBytesWithIv.Length <= IvSizeBytes)
                throw new InvalidOperationException("BehatzdaaApigeeEncryptedSecret payload is too short to contain an IV and ciphertext.");

            byte[] aesKey = Rfc2898DeriveBytes.Pbkdf2(
                password: Encoding.UTF8.GetBytes(encryptionKey),
                salt: saltBytes,
                iterations: Iterations,
                hashAlgorithm: HashAlgorithmName.SHA256,
                outputLength: KeySizeBytes);

            byte[] iv = new byte[IvSizeBytes];
            byte[] encryptedBytes = new byte[encryptedBytesWithIv.Length - IvSizeBytes];

            // Extract IV from the beginning of the encrypted payload.
            Buffer.BlockCopy(encryptedBytesWithIv, 0, iv, 0, IvSizeBytes);

            // Extract ciphertext that follows the IV.
            Buffer.BlockCopy(encryptedBytesWithIv, IvSizeBytes, encryptedBytes, 0, encryptedBytes.Length);

            using Aes aes = Aes.Create();
            aes.Key = aesKey;
            aes.IV = iv;

            using MemoryStream plainStream = new MemoryStream();

            using (CryptoStream cryptoStream = new CryptoStream(
                plainStream,
                aes.CreateDecryptor(),
                CryptoStreamMode.Write))
            {
                cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                cryptoStream.FlushFinalBlock();
            }

            return Encoding.UTF8.GetString(plainStream.ToArray());
        }
    }
}
