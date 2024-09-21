using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HealthSystem.Pharmacy.Infrastructure
{
    /// <summary>
    /// A custom value converter that encrypts and decrypts string values for database storage.
    /// </summary>
    public class EncryptionConverter : ValueConverter<string, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionConverter"/> class.
        /// </summary>
        public EncryptionConverter()
            : base(
                plainText => EncryptionHelper.Encrypt(plainText),
                encryptedText => EncryptionHelper.Decrypt(encryptedText))
        { }
    }
}
