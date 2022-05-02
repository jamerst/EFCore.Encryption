using EFCore.Encryption.Tests.Utils;

namespace EFCore.Encryption.Tests.Data;

public class HashedString : HashedValue<string>
{
    public HashedString() : base() { }
    public HashedString(string value) : base(value) { }

    protected override byte[] ToBinary(string value) {
        return BinaryConverter.ToBinary(value);
    }

    public static implicit operator HashedString(string value) => new HashedString(value);
}