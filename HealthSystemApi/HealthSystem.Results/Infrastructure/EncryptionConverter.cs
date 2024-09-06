using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HealthSystem.Results.Infrastructure
{
    public class EncryptionConverter : ValueConverter<string, string>
    {
        public EncryptionConverter()
            : base(
                plainText => EncryptionHelper.Encrypt(plainText),
                encryptedText => EncryptionHelper.Decrypt(encryptedText))
        { }
    }
}
