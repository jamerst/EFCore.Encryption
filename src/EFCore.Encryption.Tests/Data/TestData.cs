using EFCore.Encryption.Tests.Models;

namespace EFCore.Encryption.Tests.Data;

public static class TestData
{
    public static void Seed(TestContextBase db)
    {
        var newUsers = GetTestUsers();

        db.Users.AddRange(newUsers);
        db.SaveChanges();
    }

    public static IEnumerable<User> GetTestUsers()
    {
        return new[]
        {
            new User {
                FirstName = "John",
                Surname = "Smith",
                DateOfBirth = new DateOnly(1990, 01, 01),
                SomeExternalIdentifier = "ABCDEF"
            },
            new User {
                FirstName = "Bob",
                MiddleName = "Joe",
                Surname = "smith",
                DateOfBirth = new DateOnly(1996, 02, 18),
                SomeExternalIdentifier = "abcdef"
            },
            new User {
                FirstName = "Mark",
                MiddleName = "Chris",
                Surname = "",
                DateOfBirth = new DateOnly(1996, 02, 18),
                SomeExternalIdentifier = "abcdef"
            },
            new User {
                FirstName = "Jane",
                Surname = "Doe",
                DateOfBirth = new DateOnly(1980, 01, 01),
                SomeExternalIdentifier = "xyz"
            },
            new User {
                FirstName = "Kate",
                Surname = "Green",
                DateOfBirth = new DateOnly(1980, 01, 01),
                SomeExternalIdentifier = "xyz"
            },
            new User {
                FirstName = "Laura",
                Surname = "Smith",
                DateOfBirth = new DateOnly(1980, 01, 01),
                SomeExternalIdentifier = "xyz"
            },
        };
    }

    public static string GetRandomString() => new string(GetRandomCharArray(10).ToArray());

    static readonly char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
        .SelectMany(c => new char[] { c, Char.ToLower(c) })
        .Distinct()
        .ToArray();
    private static IEnumerable<char> GetRandomCharArray(int length)
    {
        for (int i = 0; i < length; i++)
        {
            yield return chars[Random.Shared.Next(0, chars.Length)];
        }
    }
}