namespace EFCore.Encryption;

/// <summary>
/// A hashed value where the value to hash is of type <typeparamref name="T"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IHashedValue<T>
{
    /// <summary>
    /// The hash as a string
    /// </summary>
    string Value { get; set; }

    /// <summary>
    /// Transform <paramref name="value"/> to the value used to compute the hash
    /// </summary>
    /// <param name="value">The value</param>
    /// <returns>Transformed value to compute the hash</returns>
    T TransformValue(T value);

    /// <summary>
    /// Compute the hash of <paramref name="value"/>
    /// </summary>
    /// <param name="value">Value to compute hash of</param>
    /// <returns>Computed hash as a string</returns>
    string GetHashString(T value);

    /// <summary>
    /// Compute the hash of <paramref name="value"/> asynchronously
    /// </summary>
    /// <param name="value">Value to compute hash of</param>
    /// <returns>Task that returns computed hash as a string</returns>
    Task<string> GetHashStringAsync(T value);

    /// <summary>
    /// Compute the hash of <paramref name="value"/> and compare it to the stored hash
    /// </summary>
    /// <param name="value">Value to compare with</param>
    /// <returns>True if the hash strings are equal</returns>
    bool HashEquals(T value);
}