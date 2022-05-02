using EFCore.Encryption.Tests.Utils;

namespace EFCore.Encryption.Tests.Data;

public class HashedDateOnly : HashedValue<DateOnly>
{
    public HashedDateOnly() : base() { }
    public HashedDateOnly(DateOnly value) : base(value) { }

    protected override byte[] ToBinary(DateOnly value) {
        return BinaryConverter.ToBinary(value);
    }

    public static implicit operator HashedDateOnly(DateOnly value) => new HashedDateOnly(value);
}