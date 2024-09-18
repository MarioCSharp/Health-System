using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace HealthSystem.Documents.Infrastructure
{
    /// <summary>
    /// A value converter for encrypting and decrypting <see cref="DateTime"/> values in the database.
    /// </summary>
    public class DateTimeEncryptionConverter : ValueConverter<DateTime, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeEncryptionConverter"/> class.
        /// </summary>
        public DateTimeEncryptionConverter()
            : base(
                plainDateTime => EncryptionHelper.Encrypt(plainDateTime),
                encryptedDateTime => EncryptionHelper.DecryptToDateTime(encryptedDateTime))
        { }
    }
}
