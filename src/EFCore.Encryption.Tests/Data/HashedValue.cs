using EFCore.Encryption.Tests.Utils;

namespace EFCore.Encryption.Tests.Data;

public abstract class HashedValue<T> : HashedValueBase<T>
{
    public HashedValue() : base() { }

    public HashedValue(T value) : base(value) { }

    public static implicit operator string(HashedValue<T> hashed) => hashed.Value;

    protected override string ComputeHash(byte[] bytes)
    {
        return SecurityUtils.Hash(bytes);
    }

    protected override Task<string> ComputeHashAsync(byte[] bytes)
    {
        return SecurityUtils.HashAsync(bytes);
    }
}