using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Encryption;

/// <summary>
/// Base class for an encrypted field implementation where the value is of type <typeparamref name="TValue"/> and the hashed value is of type <typeparamref name="THash"/>.
/// </summary>
/// <typeparam name="TValue">Type of value to encrypt</typeparam>
/// <typeparam name="THash">Hashed value type to use</typeparam>
[Owned]
public abstract class EncryptedFieldBase<TValue, THash> : IEncryptedField<TValue, THash>
    where THash : notnull, HashedValueBase<TValue>, new()
{
    /// <summary>
    /// Create a new encrypted field instance
    /// </summary>
    public EncryptedFieldBase() { }

    /// <summary>
    /// Create a new encrypted field instance using <paramref name="value"/>
    /// </summary>
    /// <param name="value"></param>
    public EncryptedFieldBase(TValue value)
    {
        Hashed = new THash();
        Hashed.Value = Hashed.GetHashString(value);

        Encrypted = Encrypt(ToBinary(value));
    }

    /// <summary>
    /// Cast the value back to the original type
    /// </summary>
    public static implicit operator TValue(EncryptedFieldBase<TValue, THash> enc) => enc.Value;

    /// <summary>
    /// The encrypted data
    /// </summary>
    public byte[] Encrypted { get; set; } = null!;

    /// <summary>
    /// Hashed version of the data
    /// </summary>
    public THash Hashed { get; set; } = null!;

    /// <summary>
    /// The decrypted value
    /// </summary>
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

    /// <summary>
    /// Get the decrypted value asynchronously
    /// </summary>
    /// <returns>Task that returns the decrypted value</returns>
    public virtual async Task<TValue> GetValueAsync()
    {
        return FromBinary(await DecryptAsync(Encrypted));
    }

    /// <summary>
    /// Set the encrypted value to <paramref name="value"/> asynchronously
    /// </summary>
    /// <param name="value">Value to encrypt</param>
    /// <returns>Task that completes when the encrypted value has been computed and set</returns>
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
    /// Compute the encrypted bytes of <paramref name="bytes"/>
    /// </summary>
    /// <param name="bytes">Bytes to encrypt</param>
    /// <returns>Encrypted bytes</returns>
    protected abstract byte[] Encrypt(byte[] bytes);

    /// <summary>
    /// Compute the encrypted bytes of <paramref name="bytes"/> asynchronously
    /// </summary>
    /// <param name="bytes">Bytes to encrypt</param>
    /// <returns>Task which returns encrypted bytes</returns>
    protected abstract Task<byte[]> EncryptAsync(byte[] bytes);

    /// <summary>
    /// Compute the decrypted bytes of <paramref name="bytes"/>
    /// </summary>
    /// <param name="bytes">Bytes to decrypt</param>
    /// <returns>Decrypted bytes</returns>
    protected abstract byte[] Decrypt(byte[] bytes);

    /// <summary>
    /// Compute the decrypted bytes of <paramref name="bytes"/> asynchronously
    /// </summary>
    /// <param name="bytes">Bytes to decrypt</param>
    /// <returns>Task which returns decrypted bytes</returns>
    protected abstract Task<byte[]> DecryptAsync(byte[] bytes);

    /// <summary>
    /// Returns a binary representation of <paramref name="value"/>
    /// </summary>
    /// <param name="value">Value to represent</param>
    /// <returns>Binary representation of the value</returns>
    protected abstract byte[] ToBinary(TValue value);

    /// <summary>
    /// Returns the value from the binary representation <paramref name="bytes"/>
    /// </summary>
    /// <param name="bytes">Binary representation of value</param>
    /// <returns>Original value</returns>
    protected abstract TValue FromBinary(byte[] bytes);

    /// <summary>
    /// Returns a string that represents the current decrypted object.
    /// </summary>
    /// <returns>A string that represents the current decrypted object.</returns>
    public override string ToString()
    {
        return Value?.ToString() ?? "";
    }
}