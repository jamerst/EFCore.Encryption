using System.Text;

using EFCore.Encryption.Tests.Data;
using EFCore.Encryption.Tests.Utils;

namespace EFCore.Encryption.Tests;

public class HashedValueTests
{
    [Fact]
    public void HashEquals_InMemory()
    {
        var users = TestData.GetTestUsers().Where(u => u.Surname.Hashed.HashEquals("smith")).ToList();

        users
            .Should()
            .OnlyContain(u => u.Surname.Value.ToLower() == "smith", "only users with a surname of Smith should be returned");
    }

    [Fact]
    public async Task GetHashStringAsync()
    {
        string testString = TestData.GetRandomString();

        var hash = new HashedString();
        string hashString = await hash.GetHashStringAsync(testString);

        hashString
            .Should()
            .Be(SecurityUtils.Hash(Encoding.UTF8.GetBytes(testString)), "hashes should be the same");
    }

    [Fact]
    public void Constructor()
    {
        string testString = TestData.GetRandomString();

        var hash = new HashedString(testString);

        hash.Value
            .Should()
            .Be(SecurityUtils.Hash(Encoding.UTF8.GetBytes(testString)), "hashes should be the same");
    }

    [Fact]
    public void ImplicitCast_In()
    {
        string testString = TestData.GetRandomString();

        HashedString hash = testString;

        hash.Value
            .Should()
            .Be(SecurityUtils.Hash(Encoding.UTF8.GetBytes(testString)), "hashes should be the same");
    }

    [Fact]
    public void ImplicitCast_Out()
    {
        string testString = TestData.GetRandomString();

        HashedString hash = new HashedString(testString);

        string value = hash;

        value
            .Should()
            .Be(SecurityUtils.Hash(Encoding.UTF8.GetBytes(testString)), "hashes should be the same");
    }
}