using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace HealthSystem.Documents.Infrastructure
{
    public class DateTimeEncryptionConverter : ValueConverter<DateTime, string>
    {
        public DateTimeEncryptionConverter()
            : base(
                plainDateTime => EncryptionHelper.Encrypt(plainDateTime),
                encryptedDateTime => EncryptionHelper.DecryptToDateTime(encryptedDateTime))
        { }
    }
}
