using System.Text.Json;

using EFCore.Encryption.Tests.Data;
using EFCore.Encryption.Tests.Json;
using EFCore.Encryption.Tests.Models;

namespace EFCore.Encryption.Tests;

public class JsonTests
{
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        Converters =
        {
            new DateOnlyConverter(),
            new EncryptedFieldConverter<EncryptedString, HashedString, string>(),
            new EncryptedFieldConverter<EncryptedName, HashedCaseInsensitiveString, string>(),
            new EncryptedFieldConverter<EncryptedDateOnly, HashedDateOnly, DateOnly>(),
        }
    };

    private const string expectedJson = @"{""Id"":0,""FirstName"":""John"",""MiddleName"":null,""Surname"":""Smith"",""DateOfBirth"":""1990-01-01"",""SomeExternalIdentifier"":""ABCDEF""}";

    [Fact]
    public void Serialize()
    {
        string json = JsonSerializer.Serialize<User>(TestData.GetTestUsers().First(), options);

        json
            .Should()
            .Be(expectedJson);
    }

    [Fact]
    public void Deserialize()
    {
        User? user = JsonSerializer.Deserialize<User>(expectedJson, options);

        user
            .Should()
            .BeEquivalentTo(
                TestData.GetTestUsers().First(),
                options => options.Excluding(ctx => ctx.Path.EndsWith("Encrypted"))
            );
    }
}