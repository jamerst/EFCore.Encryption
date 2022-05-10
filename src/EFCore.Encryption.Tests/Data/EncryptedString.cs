using System.Text;

namespace EFCore.Encryption.Tests.Data;

public class EncryptedString : EncryptedField<string, HashedString>
{
    public EncryptedString() : base() { }
    public EncryptedString(string value) : base(value) { }
    public static implicit operator EncryptedString(string val) => new EncryptedString(val);

    protected override string FromBinary(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    protected override byte[] ToBinary(string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }
}