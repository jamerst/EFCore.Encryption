using EFCore.Encryption.Tests.Data;
using EFCore.Encryption.Tests.Models;

namespace EFCore.Encryption.Tests;

public abstract class DbTestBase<TFixture, TContext> : IClassFixture<TFixture>
    where TFixture : TestFixtureBase<TContext>
    where TContext : TestContextBase, new()
{
    protected TContext db { get; set; }

    public DbTestBase(TFixture fixture)
    {
        db = fixture.DbContext;
    }

    #region Reading from DB
    [Fact]
    public async Task Select_DecryptedValue()
    {
        var count = await db.Users.CountAsync();

        var encryptedNames = await db.Users.AsNoTracking().Select(u => u.FirstName).ToListAsync();
        encryptedNames
            .Should()
            .BeOfType<List<EncryptedName>>()
            .And.HaveCount(count);

        var values = encryptedNames.Select(n => n.Value).ToList();
        values
            .Should()
            .BeOfType<List<string>>()
            .And.NotContainNulls();
    }

    [Fact]
    public async Task Select_Hashed()
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
    #endregion

    #region Creating
    [Fact]
    public async Task Create()
    {
        User createUser()
        {
            return new User
            {
                FirstName = "Jess",
                MiddleName = "Rebecca",
                Surname = "Thompson",
                DateOfBirth = new DateOnly(1980, 01, 01),
                SomeExternalIdentifier = "CreatedUser"
            };
        };

        Func<Task> addToDb = async () =>
        {
            User dbUser = createUser();
            db.Users.Add(dbUser);
            await db.SaveChangesAsync();
        };

        await addToDb.Should().NotThrowAsync();

        User expected = createUser();
        User dbUser = await db.Users.FirstAsync(u => u.SomeExternalIdentifier.Hashed.HashEquals("CreatedUser"));

        dbUser
            .Should()
            .BeEquivalentTo(
                expected,
                options => options // exclude the ID and encrypted data since the encryption is non-deterministic
                    .Excluding(u => u.Id)
                    .Excluding(u => u.FirstName.Encrypted)
                    .Excluding(u => u.MiddleName!.Encrypted)
                    .Excluding(u => u.Surname.Encrypted)
                    .Excluding(u => u.DateOfBirth.Encrypted)
                    .Excluding(u => u.SomeExternalIdentifier.Encrypted),
                "the created entity should be the same"
            );
    }
    #endregion

    #region HashEquals Non case-sensitive
    [Fact]
    public async Task NonCaseSensitive_SameCase()
    {
        var users = await db.Users.Where(u => u.Surname.Hashed.HashEquals("Smith")).ToListAsync();

        users
            .Should()
            .OnlyContain(u => u.Surname.Value.ToLower() == "smith", "only users with a surname of Smith should be returned");
    }

    [Fact]
    public async Task NonCaseSensitive_DifferentCase()
    {
        var users = await db.Users.Where(u => u.Surname.Hashed.HashEquals("smith")).ToListAsync();

        users
            .Should()
            .OnlyContain(u => u.Surname.Value.ToLower() == "smith", "only users with a surname of Smith should be returned");
    }

    [Fact]
    public async Task NonCaseSensitive_SameCase_NoResults()
    {
        var users = await db.Users.Where(u => u.Surname.Hashed.HashEquals("Brown")).ToListAsync();

        users
            .Should()
            .BeEmpty("no users have a surname of Brown");
    }

    [Fact]
    public async Task NonCaseSensitive_DifferentCase_NoResults()
    {
        var users = await db.Users.Where(u => u.Surname.Hashed.HashEquals("BROWN")).ToListAsync();

        users
            .Should()
            .BeEmpty("no users have a surname of Brown");
    }
    #endregion

    #region HashEquals Case-sensitive
    [Fact]
    public async Task CaseSensitive()
    {
        var users = await db.Users.Where(u => u.SomeExternalIdentifier.Hashed.HashEquals("abcdef")).ToListAsync();

        users
            .Should()
            .OnlyContain(u => u.SomeExternalIdentifier.Value == "abcdef", "only users with an external identifier of abcdef should be returned");
    }

    [Fact]
    public async Task CaseSensitive_NoResults()
    {
        var users = await db.Users.Where(u => u.SomeExternalIdentifier.Hashed.HashEquals("None")).ToListAsync();

        users
            .Should()
            .BeEmpty("no users have an external identifier of None");
    }

    [Fact]
    public async Task CaseSensitive_DifferentCase_NoResults()
    {
        var users = await db.Users.Where(u => u.SomeExternalIdentifier.Hashed.HashEquals("ABCdef")).ToListAsync();

        users
            .Should()
            .BeEmpty("no users have an external identifier of ABCdef");
    }
    #endregion

    #region HashEquals DateOnly
    public async Task DateOnly()
    {
        var users = await db.Users.Where(u => u.DateOfBirth.Hashed.HashEquals(new DateOnly(1980, 01, 01))).ToListAsync();

        users
            .Should()
            .OnlyContain(u => u.DateOfBirth.Value == new DateOnly(1980, 01, 01), "only users born on 1st Jan 1980 should be returned");
    }

    public async Task DateOnly_NoResults()
    {
        var users = await db.Users.Where(u => u.DateOfBirth.Hashed.HashEquals(new DateOnly(1970, 01, 01))).ToListAsync();

        users
            .Should()
            .BeEmpty("no users were born on 1st Jan 1970");
    }
    #endregion
}