using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Encryption;

/// <summary>
/// An encrypted field implementation where the value is of type <typeparamref name="TValue"/> and the hashed value is of type <typeparamref name="THash"/>.
/// </summary>
/// <typeparam name="TValue">Type of value to encrypt</typeparam>
/// <typeparam name="THash">Hashed value type to use</typeparam>
public interface IEncryptedField<TValue, THash>
    where THash : IHashedValue<TValue>, new()
{
    /// <summary>
    /// The encrypted data
    /// </summary>
    byte[] Encrypted { get; set; }

    /// <summary>
    /// Hashed version of the data
    /// </summary>
    THash Hashed { get; set; }

    /// <summary>
    /// The decrypted value
    /// </summary>
    [NotMapped]
    TValue Value { get; set; }

    /// <summary>
    /// Get the decrypted value asynchronously
    /// </summary>
    /// <returns>Task that returns the decrypted value</returns>
    Task<TValue> GetValueAsync();

    /// <summary>
    /// Set the encrypted value to <paramref name="value"/> asynchronously
    /// </summary>
    /// <param name="value">Value to encrypt</param>
    /// <returns>Task that completes when the encrypted value has been computed and set</returns>
    Task SetValueAsync(TValue value);
}