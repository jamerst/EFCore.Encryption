using EFCore.Encryption.Tests.Utils;

namespace EFCore.Encryption.Tests.Data;

public abstract class EncryptedField<TValue, THash> : EncryptedFieldBase<TValue, THash>
    where THash : HashedValueBase<TValue>, new()
{
    public EncryptedField() { }

    public EncryptedField(TValue value) : base(value) { }

    public static implicit operator TValue(EncryptedField<TValue, THash> enc) => enc.Value;

    protected override byte[] Decrypt(byte[] bytes)
    {
        return SecurityUtils.Decrypt(bytes);
    }

    protected override Task<byte[]> DecryptAsync(byte[] bytes)
    {
        return SecurityUtils.DecryptAsync(bytes);
    }

    protected override byte[] Encrypt(byte[] bytes)
    {
        return SecurityUtils.Encrypt(bytes);
    }

    protected override Task<byte[]> EncryptAsync(byte[] bytes)
    {
        return SecurityUtils.EncryptAsync(bytes);
    }
}