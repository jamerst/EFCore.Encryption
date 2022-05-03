using System.Text;

using EFCore.Encryption.Tests.Data;
using EFCore.Encryption.Tests.Utils;

namespace EFCore.Encryption.Tests;

public class EncryptedFieldTests
{
    [Fact]
    public void Value_Get()
    {
        string testString = TestData.GetRandomString();

        var enc = new EncryptedString(testString);

        enc.Value
            .Should()
            .Be(testString);
    }

    [Fact]
    public void Value_Set()
    {
        string testString = TestData.GetRandomString();

        var enc = new EncryptedString("Initial");
        enc.Value = testString;

        enc.Encrypted
            .Should()
            .NotBeEmpty("encrypted data should be set");

        var decrypted = Encoding.UTF8.GetString(SecurityUtils.Decrypt(enc.Encrypted));

        decrypted
            .Should()
            .Be(testString, "decrypted value should be the same");
    }

    [Fact]
    public async Task GetValueAsync()
    {
        string testString = TestData.GetRandomString();

        var enc = new EncryptedString(testString);

        var value = await enc.GetValueAsync();

        value
            .Should()
            .Be(testString, "decrypted value should be the same");
    }

    [Fact]
    public async Task SetValueAsync()
    {
        string testString = TestData.GetRandomString();

        var enc = new EncryptedString("Initial");

        await enc.SetValueAsync(testString);

        enc.Encrypted
            .Should()
            .NotBeEmpty("encrypted data should be set");

        enc.Value
            .Should()
            .Be(testString, "decrypted value should be the same");
    }

    [Fact]
    public void Constructor()
    {
        string testString = TestData.GetRandomString();

        var enc = new EncryptedString(testString);

        enc.Value
            .Should()
            .Be(testString, "decrypted value should be the same");
    }

    [Fact]
    public void ImplicitCast_In()
    {
        string testString = TestData.GetRandomString();

        EncryptedString enc = testString;

        enc.Value
            .Should()
            .Be(testString, "decrypted value should be the same");
    }

    [Fact]
    public void ImplicitCast_Out()
    {
        string testString = TestData.GetRandomString();

        EncryptedString enc = new EncryptedString(testString);

        string value = enc;

        value
            .Should()
            .Be(testString, "decrypted value should be the same");
    }

    [Fact]
    public void Test_ToString()
    {
        string testString = TestData.GetRandomString();

        var enc = new EncryptedString(testString);

        enc.ToString()
            .Should()
            .Be(testString);
    }
}