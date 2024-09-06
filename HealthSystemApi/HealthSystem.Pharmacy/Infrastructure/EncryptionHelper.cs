using System.Security.Cryptography;
using System.Text;
using DotNetEnv;

namespace HealthSystem.Pharmacy.Infrastructure
{
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

            string keyBase64 = Environment.GetEnvironmentVariable("ENCRYPTION_KEY_PHARMACY");
            string ivBase64 = Environment.GetEnvironmentVariable("ENCRYPTION_IV_PHARMACY");

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
    }
}
