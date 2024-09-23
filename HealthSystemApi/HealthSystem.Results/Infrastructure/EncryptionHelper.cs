using DotNetEnv;
using System.Security.Cryptography;
using System.Text;

namespace HealthSystem.Results.Infrastructure
{
    /// <summary>
    /// Provides methods to encrypt and decrypt text and byte arrays using AES encryption.
    /// </summary>
    public class EncryptionHelper
    {
        private static readonly byte[] key;
        private static readonly byte[] iv;

        /// <summary>
        /// Static constructor for <see cref="EncryptionHelper"/> that initializes the encryption key and IV from environment variables.
        /// </summary>
        /// <exception cref="FileNotFoundException">Thrown when the environment file is not found.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the encryption key or IV is not found in the environment variables.</exception>
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

            string keyBase64 = Environment.GetEnvironmentVariable("ENCRYPTION_KEY_RESULTS");
            string ivBase64 = Environment.GetEnvironmentVariable("ENCRYPTION_IV_RESULTS");

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
        /// Encrypts the specified plain text string.
        /// </summary>
        /// <param name="plainText">The plain text to be encrypted.</param>
        /// <returns>The encrypted text in Base64 format.</returns>
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
        /// Decrypts the specified encrypted text string.
        /// </summary>
        /// <param name="encryptedText">The encrypted text in Base64 format.</param>
        /// <returns>The decrypted plain text string.</returns>
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
        /// Encrypts the specified byte array.
        /// </summary>
        /// <param name="data">The byte array to be encrypted.</param>
        /// <returns>The encrypted byte array.</returns>
        public static byte[] Encrypt(byte[] data)
        {
            if (data == null || data.Length == 0) return data;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(data, 0, data.Length);
                    }

                    return ms.ToArray();
                }
            }
        }

        /// <summary>
        /// Decrypts the specified encrypted byte array.
        /// </summary>
        /// <param name="encryptedData">The encrypted byte array.</param>
        /// <returns>The decrypted byte array.</returns>
        public static byte[] Decrypt(byte[] encryptedData)
        {
            if (encryptedData == null || encryptedData.Length == 0) return encryptedData;

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(encryptedData))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var outputMs = new MemoryStream())
                {
                    cs.CopyTo(outputMs);
                    return outputMs.ToArray();
                }
            }
        }
    }
}
