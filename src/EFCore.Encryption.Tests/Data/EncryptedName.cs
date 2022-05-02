using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFCore.Encryption.Tests.Data;

public class EncryptedName : EncryptedField<string, HashedCaseInsensitiveString>
{
    public EncryptedName() { }
    public EncryptedName(string value) : base(value) { }
    public static implicit operator EncryptedName(string val) => new EncryptedName(val);

    // Need to override in order to still detect changes if casing changes
    [NotMapped]
    public override string Value
    {
        get
        {
            return FromBinary(Decrypt(Encrypted));
        }

        set
        {
            var caseSensitiveHash = new HashedString();
            string oldHash = caseSensitiveHash.GetHashString(Value);
            string newHash = caseSensitiveHash.GetHashString(value);

            if (newHash != oldHash)
            {
                Hashed.Value = Hashed.GetHashString(value);
                Encrypted = Encrypt(ToBinary(value));
            }
        }
    }

    // Need to override in order to still detect changes if casing changes
    public override async Task SetValueAsync(string value)
    {
        var caseSensitiveHash = new HashedString();
        string oldHash = await caseSensitiveHash.GetHashStringAsync(Value);
        string newHash = await caseSensitiveHash.GetHashStringAsync(value);

        if (newHash != oldHash)
        {
            Hashed.Value = await Hashed.GetHashStringAsync(value);
            Encrypted = await EncryptAsync(ToBinary(value));
        }
    }

    protected override string FromBinary(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }

    protected override byte[] ToBinary(string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }
}