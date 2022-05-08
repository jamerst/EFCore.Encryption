using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.Encryption;

public abstract class HashedValueBase<T> : IHashedValue<T> {
    public string Value { get; set; } = null!;

    public HashedValueBase() { }
    public HashedValueBase(T value)
    {
        Value = GetHashString(value);
    }
    public static implicit operator string(HashedValueBase<T> hashed) => hashed.Value;

    public bool HashEquals([NotParameterized] T value)
    {
        return Value == GetHashString(value);
    }

    public virtual T GetHashValue(T value)
    {
        return value;
    }

    public virtual string GetHashString(T value)
    {
        return ComputeHash(ToBinary(GetHashValue(value)));
    }

    public virtual async Task<string> GetHashStringAsync(T value)
    {
        return await ComputeHashAsync(ToBinary(GetHashValue(value)));
    }

    /// <summary>
    /// Convert a value to binary form
    /// </summary>
    /// <param name="value">Value to convert</param>
    /// <returns>Binary form of the value</returns>
    protected abstract byte[] ToBinary(T value);

    /// <summary>
    /// Compute the hash of binary data
    /// </summary>
    /// <param name="bytes">Bytes to hash</param>
    /// <returns>Hash of bytes as a string</returns>
    protected abstract string ComputeHash(byte[] bytes);

    /// <summary>
    /// Compute the hash of binary data asynchronously
    /// </summary>
    /// <param name="bytes">Bytes to hash</param>
    /// <returns>Task which returns hash of bytes as a string</returns>
    protected abstract Task<string> ComputeHashAsync(byte[] bytes);
}