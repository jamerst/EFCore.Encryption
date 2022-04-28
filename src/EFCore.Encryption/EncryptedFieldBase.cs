using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Encryption;

[Owned]
public abstract class EncryptedFieldBase<TValue, THash> : IEncryptedField<TValue, THash>
    where THash : notnull, HashedValueBase<TValue>, new()
{
    public EncryptedFieldBase() { }

    public EncryptedFieldBase(TValue value)
    {
        Hashed = new THash();
        Hashed.Value = Hashed.GetHashString(value);

        Encrypted = Encrypt(ToBinary(value));
    }

    public static implicit operator TValue(EncryptedFieldBase<TValue, THash> enc) => enc.Value;

    public byte[] Encrypted { get; set; } = null!;

    public THash Hashed { get; set; } = null!;

    [NotMapped]
    public virtual TValue Value
    {
        get
        {
            return FromBinary(Decrypt(Encrypted));
        }

        set
        {
            string newHash = Hashed.GetHashString(value);

            if (newHash != Hashed.Value)
            {
                Encrypted = Encrypt(ToBinary(value));
                Hashed.Value = newHash;
            }
        }
    }

    public virtual async Task<TValue> GetValueAsync()
    {
        return FromBinary(await DecryptAsync(Encrypted));
    }

    public virtual async Task SetValueAsync(TValue value)
    {
        string newHash = await Hashed.GetHashStringAsync(value);

        if (newHash != Hashed.Value)
        {
            Hashed.Value = newHash;
            Encrypted = await EncryptAsync(ToBinary(value));
        }
    }

    /// <summary>
    /// Encrypt binary data
    /// </summary>
    /// <param name="bytes">Bytes to encrypt</param>
    /// <returns>Encrypted bytes</returns>
    protected abstract byte[] Encrypt(byte[] bytes);

    /// <summary>
    /// Encrypt binary data asynchronously
    /// </summary>
    /// <param name="bytes">Bytes to encrypt</param>
    /// <returns>Task which returns encrypted bytes</returns>
    protected abstract Task<byte[]> EncryptAsync(byte[] bytes);

    /// <summary>
    /// Decrypt binary data
    /// </summary>
    /// <param name="bytes">Bytes to decrypt</param>
    /// <returns>Decrypted bytes</returns>
    protected abstract byte[] Decrypt(byte[] bytes);

    /// <summary>
    /// Decrypt binary data asynchronously
    /// </summary>
    /// <param name="bytes">Bytes to decrypt</param>
    /// <returns>Task which returns decrypted bytes</returns>
    protected abstract Task<byte[]> DecryptAsync(byte[] bytes);

    /// <summary>
    /// Convert a value to binary form
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>Binary form of the value</returns>
    protected abstract byte[] ToBinary(TValue value);

    /// <summary>
    /// Convert the binary form back to the original type
    /// </summary>
    /// <param name="bytes">Binary data to convert</param>
    /// <returns>Converted data</returns>
    protected abstract TValue FromBinary(byte[] bytes);

    public override string ToString()
    {
        return Value?.ToString() ?? "";
    }
}