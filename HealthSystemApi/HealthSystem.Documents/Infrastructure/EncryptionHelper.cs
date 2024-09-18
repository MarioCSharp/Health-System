using DotNetEnv;
using System.Security.Cryptography;
using System.Text;

namespace HealthSystem.Documents.Infrastructure
{
    /// <summary>
    /// Provides methods for encrypting and decrypting data using AES encryption.
    /// </summary>
    public class EncryptionHelper
    {
        private static readonly byte[] key;
        private static readonly byte[] iv;

        static EncryptionHelper()
        {
            string envFilePath = Environment.GetEnvironmentVariable("ENCRYPTION_ENV_PATH")
                             ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data-encryption.env");

            Console.WriteLine($"Loading env file from: {envFilePath}");

            if (!File.Exists(envFilePath))
            {
                throw new FileNotFoundException($"The env file at path {envFilePath} was not found.");
            }

            Env.Load(envFilePath);

            string keyBase64 = Environment.GetEnvironmentVariable("ENCRYPTION_KEY_DOCUMENTS");
            string ivBase64 = Environment.GetEnvironmentVariable("ENCRYPTION_IV_DOCUMENTS");

            if (string.IsNullOrEmpty(keyBase64))
            {
                throw new ArgumentNullException(nameof(keyBase64), "Encryption key not found in environment variables.");
            }

            if (string.IsNullOrEmpty(ivBase64))
            {
                throw new ArgumentNullException(nameof(ivBase64), "Encryption IV not found in environment variables.");
            }

            key = Convert.FromBase64String(keyBase64);
            iv = Convert.FromBase64String(ivBase64);
        }

        /// <summary>
        /// Encrypts a plain text string using AES encryption.
        /// </summary>
        /// <param name="plainText">The plain text to encrypt.</param>
        /// <returns>The encrypted text in base64 format.</returns>
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs, Encoding.UTF8))
                    {
                        sw.Write(plainText);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypts an encrypted text string using AES encryption.
        /// </summary>
        /// <param name="encryptedText">The encrypted text in base64 format.</param>
        /// <returns>The decrypted plain text.</returns>
        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText)) return encryptedText;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Encrypts a byte array using AES encryption.
        /// </summary>
        /// <param name="plainBytes">The byte array to encrypt.</param>
        /// <returns>The encrypted byte array.</returns>
        public static byte[] Encrypt(byte[] plainBytes)
        {
            if (plainBytes == null || plainBytes.Length == 0) return plainBytes;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                    }

                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Decrypts an encrypted byte array using AES encryption.
        /// </summary>
        /// <param name="encryptedBytes">The encrypted byte array.</param>
        /// <returns>The decrypted byte array.</returns>
        public static byte[] Decrypt(byte[] encryptedBytes)
        {
            if (encryptedBytes == null || encryptedBytes.Length == 0) return encryptedBytes;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(encryptedBytes))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (var decryptedStream = new MemoryStream())
                    {
                        cs.CopyTo(decryptedStream);
                        return decryptedStream.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts a <see cref="DateTime"/> object by converting it to an ISO 8601 string and then encrypting it.
        /// </summary>
        /// <param name="dateTime">The <see cref="DateTime"/> object to encrypt.</param>
        /// <returns>The encrypted date time as a base64 string.</returns>
        public static string Encrypt(DateTime dateTime)
        {
            string dateTimeString = dateTime.ToString("o");
            return Encrypt(dateTimeString);
        }

        /// <summary>
        /// Decrypts an encrypted date time string and converts it back to a <see cref="DateTime"/> object.
        /// </summary>
        /// <param name="encryptedDateTime">The encrypted date time string.</param>
        /// <returns>The decrypted <see cref="DateTime"/> object.</returns>
        public static DateTime DecryptToDateTime(string encryptedDateTime)
        {
            string decryptedText = Decrypt(encryptedDateTime);
            return DateTime.Parse(decryptedText);
        }
    }
}
