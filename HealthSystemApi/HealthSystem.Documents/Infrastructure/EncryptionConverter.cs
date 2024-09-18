using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HealthSystem.Documents.Infrastructure
{
    /// <summary>
    /// A value converter for encrypting and decrypting <see cref="string"/> values in the database.
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
