using EFCore.Encryption.Tests.Data;
using EFCore.Encryption.Tests.Models;

namespace EFCore.Encryption.Tests;

public abstract class TestBase<TFixture, TContext> : IClassFixture<TFixture>
    where TFixture : TestFixtureBase<TContext>
    where TContext : TestContextBase, new()
{
    protected TContext db { get; set; }

    public TestBase(TFixture fixture)
    {
        db = fixture.DbContext;
    }

    [Fact]
    public async Task SelectHashed()
    {
        var count = await db.Users.CountAsync();

        var encryptedNames = await db.Users.AsNoTracking().Select(u => u.FirstName).ToListAsync();
        encryptedNames
            .Should()
            .BeOfType<List<EncryptedName>>()
            .And.HaveCount(count);

        var hashes = encryptedNames.Select(n => n.Hashed).ToList();
        hashes
            .Should()
            .BeOfType<List<HashedCaseInsensitiveString>>()
            .And.NotContainNulls();
    }

    #region Non case-sensitive
    [Fact]
    public async Task NonCaseSensitive_SameCase_SingleResult()
    {
        var users = await db.Users.Where(u => u.FirstName.Hashed.HashEquals("John")).ToListAsync();

        users
            .Should()
            .BeOfType<List<User>>()
            .And
            .HaveCount(1, "exactly one user is called John")
            .And
            .OnlyContain(u => u.FirstName.Value.ToLower() == "john", "only users called John should be returned");
    }

    [Fact]
    public async Task NonCaseSensitive_DifferentCase_SingleResult()
    {
        var users = await db.Users.Where(u => u.FirstName.Hashed.HashEquals("joHn")).ToListAsync();

        users
            .Should()
            .BeOfType<List<User>>()
            .And
            .HaveCount(1, "exactly one user is called John")
            .And
            .OnlyContain(u => u.FirstName.Value.ToLower() == "john", "only users called John should be returned");
    }

    [Fact]
    public async Task NonCaseSensitive_SameCase_MultipleResults()
    {
        var users = await db.Users.Where(u => u.Surname.Hashed.HashEquals("Smith")).ToListAsync();

        users
            .Should()
            .BeOfType<List<User>>()
            .And
            .OnlyContain(u => u.Surname.Value.ToLower() == "smith", "only users with a surname of Smith should be returned");
    }

    [Fact]
    public async Task NonCaseSensitive_DifferentCase_MultipleResults()
    {
        var users = await db.Users.Where(u => u.Surname.Hashed.HashEquals("smith")).ToListAsync();

        users
            .Should()
            .BeOfType<List<User>>()
            .And
            .OnlyContain(u => u.Surname.Value.ToLower() == "smith", "only users with a surname of Smith should be returned");
    }

    [Fact]
    public async Task NonCaseSensitive_SameCase_NoResults()
    {
        var users = await db.Users.Where(u => u.Surname.Hashed.HashEquals("Brown")).ToListAsync();

        users
            .Should()
            .BeOfType<List<User>>()
            .And
            .BeEmpty("no users have a surname of Brown");
    }

    [Fact]
    public async Task NonCaseSensitive_DifferentCase_NoResults()
    {
        var users = await db.Users.Where(u => u.Surname.Hashed.HashEquals("BROWN")).ToListAsync();

        users
            .Should()
            .BeOfType<List<User>>()
            .And
            .BeEmpty("no users have a surname of Brown");
    }
    #endregion

    #region Case-sensitive
    [Fact]
    public async Task CaseSensitive_SingleResult()
    {
        var users = await db.Users.Where(u => u.SomeExternalIdentifier.Hashed.HashEquals("ABCDEF")).ToListAsync();

        users
            .Should()
            .BeOfType<List<User>>()
            .And
            .HaveCount(1, "exactly one user has an external identifier of ABCDEF")
            .And
            .OnlyContain(u => u.SomeExternalIdentifier.Value == "ABCDEF", "only users with an external identifier of ABCDEF should be returned");
    }

    [Fact]
    public async Task CaseSensitive_MultipleResults()
    {
        var users = await db.Users.Where(u => u.SomeExternalIdentifier.Hashed.HashEquals("abcdef")).ToListAsync();

        users
            .Should()
            .BeOfType<List<User>>()
            .And
            .OnlyContain(u => u.SomeExternalIdentifier.Value == "abcdef", "only users with an external identifier of abcdef should be returned");
    }

    [Fact]
    public async Task CaseSensitive_NoResults()
    {
        var users = await db.Users.Where(u => u.SomeExternalIdentifier.Hashed.HashEquals("xyz")).ToListAsync();

        users
            .Should()
            .BeOfType<List<User>>()
            .And
            .BeEmpty("no users have an external identifier of xyz");
    }

    [Fact]
    public async Task CaseSensitive_DifferentCase_NoResults()
    {
        var users = await db.Users.Where(u => u.SomeExternalIdentifier.Hashed.HashEquals("ABCdef")).ToListAsync();

        users
            .Should()
            .BeOfType<List<User>>()
            .And
            .BeEmpty("no users have an external identifier of ABCdef");
    }
    #endregion
}