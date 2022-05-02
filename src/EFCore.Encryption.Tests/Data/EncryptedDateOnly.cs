using System.Text;

using EFCore.Encryption.Tests.Utils;

namespace EFCore.Encryption.Tests.Data;

public class EncryptedDateOnly : EncryptedField<DateOnly, HashedDateOnly>
{
    public EncryptedDateOnly() { }
    public EncryptedDateOnly(DateOnly value) : base(value) { }

    public static implicit operator EncryptedDateOnly(DateOnly date) => new EncryptedDateOnly(date);

    protected override DateOnly FromBinary(byte[] bytes)
    {
        return DateOnly.ParseExact(Encoding.UTF8.GetString(bytes), "yyyy-MM-dd");
    }

    protected override byte[] ToBinary(DateOnly value)
    {
        return BinaryConverter.ToBinary(value);
    }
}