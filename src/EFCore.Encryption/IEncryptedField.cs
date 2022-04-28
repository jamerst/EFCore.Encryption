using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Encryption;

public interface IEncryptedField<TValue, THash>
    where THash : IHashedValue<TValue>, new()
{
    /// <summary>
    /// The encrypted data
    /// </summary>
    byte[] Encrypted { get; set; }

    /// <summary>
    /// Hashed version of the data. Use this for querying or comparison.
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
    /// Set the encrypted value asynchronously
    /// </summary>
    /// <param name="value">Value to encrypt</param>
    /// <returns>Task that completes when the value has been encrypted and stored</returns>
    Task SetValueAsync(TValue value);
}