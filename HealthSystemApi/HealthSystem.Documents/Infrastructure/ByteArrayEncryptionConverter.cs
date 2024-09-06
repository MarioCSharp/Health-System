using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HealthSystem.Documents.Infrastructure
{
    public class ByteArrayEncryptionConverter : ValueConverter<byte[], byte[]>
    {
        public ByteArrayEncryptionConverter()
            : base(
                plainBytes => EncryptionHelper.Encrypt(plainBytes),
                encryptedBytes => EncryptionHelper.Decrypt(encryptedBytes))
        { }
    }
}
