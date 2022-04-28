namespace EFCore.Encryption;

public interface IHashedValue<T>
{
    /// <summary>
    /// The hash as a string
    /// </summary>
    string Value { get; set; }

    /// <summary>
    /// Get the value used to compute the hash
    /// </summary>
    /// <param name="value">The value</param>
    /// <returns>The value used to compute the hash</returns>
    T GetHashValue(T value);

    /// <summary>
    /// Compute the hash of a value
    /// </summary>
    /// <param name="value">Value to compute hash of</param>
    /// <returns>Computed hash as a string</returns>
    string GetHashString(T value);

    /// <summary>
    /// Compute the hash of a value, asynchronously
    /// </summary>
    /// <param name="value">Value to compute hash of</param>
    /// <returns>Computed hash as a string</returns>
    Task<string> GetHashStringAsync(T value);

    /// <summary>
    /// Compute the hash of a value and compare it to the stored hash
    /// </summary>
    /// <param name="value">Value to compare with</param>
    /// <returns>True if the hash strings are equal</returns>
    bool HashEquals(T value);
}