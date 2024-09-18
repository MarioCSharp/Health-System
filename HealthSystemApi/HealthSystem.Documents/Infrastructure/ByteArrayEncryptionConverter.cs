using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HealthSystem.Documents.Infrastructure
{
    /// <summary>
    /// A value converter for encrypting and decrypting byte arrays in the database.
    /// </summary>
    public class ByteArrayEncryptionConverter : ValueConverter<byte[], byte[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ByteArrayEncryptionConverter"/> class.
        /// </summary>
        public ByteArrayEncryptionConverter()
            : base(
                plainBytes => EncryptionHelper.Encrypt(plainBytes),
                encryptedBytes => EncryptionHelper.Decrypt(encryptedBytes))
        { }
    }
}
