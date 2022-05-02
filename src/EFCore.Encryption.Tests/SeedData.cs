using EFCore.Encryption.Tests.Data;
using EFCore.Encryption.Tests.Models;

namespace EFCore.Encryption.Tests;

public static class SeedData
{
    public static void Seed(TestContextBase db)
    {
        var newUsers = new[]
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
            }
        };

        db.Users.AddRange(newUsers);
        db.SaveChanges();
    }
}