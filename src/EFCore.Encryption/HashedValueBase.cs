using Microsoft.EntityFrameworkCore.Query;

namespace EFCore.Encryption;

/// <summary>
/// A base class for a hashed value where the value to hash is of type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class HashedValueBase<T> : IHashedValue<T>
{
    /// <summary>
    /// The hash as a string
    /// </summary>
    public string Value { get; set; } = null!;

    /// <summary>
    /// Create a new hashed value instance
    /// </summary>
    public HashedValueBase() { }

    /// <summary>
    /// Create a new hashed value instance using <paramref name="value"/>
    /// </summary>
    /// <param name="value">Value to initialise with</param>
    public HashedValueBase(T value)
    {
        Value = GetHashString(value);
    }

    /// <summary>
    /// Cast the hashed value to the string
    /// </summary>
    public static implicit operator string(HashedValueBase<T> hashed) => hashed.Value;

    /// <summary>
    /// Compute the hash of <paramref name="value"/> and compare it to the stored hash
    /// </summary>
    /// <param name="value">Value to compare with</param>
    /// <returns>True if the hash strings are equal</returns>
    public bool HashEquals([NotParameterized] T value)
    {
        return Value == GetHashString(value);
    }

    /// <summary>
    /// Transform <paramref name="value"/> to the value used to compute the hash
    /// </summary>
    /// <param name="value">The value</param>
    /// <returns>Transformed value to compute the hash</returns>
    public virtual T TransformValue(T value)
    {
        return value;
    }

    /// <summary>
    /// Compute the hash of <paramref name="value"/>
    /// </summary>
    /// <param name="value">Value to compute hash of</param>
    /// <returns>Computed hash as a string</returns>
    public virtual string GetHashString(T value)
    {
        return ComputeHash(ToBinary(TransformValue(value)));
    }

    /// <summary>
    /// Compute the hash of <paramref name="value"/> asynchronously
    /// </summary>
    /// <param name="value">Value to compute hash of</param>
    /// <returns>Task that returns computed hash as a string</returns>
    public virtual async Task<string> GetHashStringAsync(T value)
    {
        return await ComputeHashAsync(ToBinary(TransformValue(value)));
    }

    /// <summary>
    /// Returns a binary representation of <paramref name="value"/>
    /// </summary>
    /// <param name="value">Value to represent</param>
    /// <returns>Binary representation of the value</returns>
    protected abstract byte[] ToBinary(T value);

    /// <summary>
    /// Compute the hash of <paramref name="bytes"/>
    /// </summary>
    /// <param name="bytes">Bytes to hash</param>
    /// <returns>Hash as a string</returns>
    protected abstract string ComputeHash(byte[] bytes);

    /// <summary>
    /// Compute the hash of <paramref name="bytes"/> asynchronously
    /// </summary>
    /// <param name="bytes">Bytes to hash</param>
    /// <returns>Task which returns hash as a string</returns>
    protected abstract Task<string> ComputeHashAsync(byte[] bytes);
}