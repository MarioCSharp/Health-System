using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HealthSystem.Results.Infrastructure
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
