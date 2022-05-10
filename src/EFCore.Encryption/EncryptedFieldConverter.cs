using System.Text.Json;
using System.Text.Json.Serialization;

namespace EFCore.Encryption;

/// <summary>
/// JsonConverter for <typeparamref name="TEnc"/>
/// </summary>
/// <typeparam name="TEnc">Encrypted field type</typeparam>
/// <typeparam name="THash">Hashed value type</typeparam>
/// <typeparam name="TValue">Value type</typeparam>
public class EncryptedFieldConverter<TEnc, THash, TValue> : JsonConverter<TEnc>
    where TEnc : IEncryptedField<TValue, THash>, new()
    where THash : IHashedValue<TValue>, new()
{
    #pragma warning disable CS1591
    public override void Write(Utf8JsonWriter writer, TEnc value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Value, typeof(TValue), options);
    }

    public override TEnc? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        TValue? value = JsonSerializer.Deserialize<TValue>(ref reader, options);

        if (value == null)
        {
            return default(TEnc);
        }
        else
        {
            return new TEnc()
            {
                Value = value
            };
        }

    }
}