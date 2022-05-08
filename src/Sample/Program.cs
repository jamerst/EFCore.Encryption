global using Microsoft.EntityFrameworkCore;
global using EFCore.Encryption;

using Sample.Data;
using Sample.Models;

using (var db = new SampleContext()) {
    await db.Database.EnsureCreatedAsync();

    if (!await db.Users.AnyAsync()) {
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
                Surname = "Smith",
                DateOfBirth = new DateOnly(1996, 02, 18),
                SomeExternalIdentifier = "abcdef"
            }
        };

        db.Users.AddRange(newUsers);
        await db.SaveChangesAsync();
    }

    string val = "smiTh";

    // will return both users even though search string is cased differently
    Console.WriteLine("Users with a surname of 'Smith':");
    await foreach (var user in db.Users.Where(u => u.Surname.Hashed.HashEquals(val)).AsAsyncEnumerable()) {
        Console.WriteLine(user.ToOutputString());
    }

    Console.WriteLine("\nUsers with a middle name:");
    await foreach (var user in db.Users.Where(u => u.MiddleName != null).AsAsyncEnumerable()) {
        Console.WriteLine(user.ToOutputString());
    }

    Console.WriteLine("\nUsers born on 01/01/1990:");
    await foreach (var user in db.Users.Where(u => u.DateOfBirth.Hashed.HashEquals(new DateOnly(1990, 01, 01))).AsAsyncEnumerable()) {
        Console.WriteLine(user.ToOutputString());
    }

    // Will only return Bob Joe Smith because the hash for it is case-sensitive
    // AsNoTracking() is required when there isn't a tracked entity to associate the Owned entity with
    Console.WriteLine("\nFirst names of user with SomeExternalIdentifier='abcdef'");
    await foreach (var name in db.Users.AsNoTracking().Where(u => u.SomeExternalIdentifier.Hashed.HashEquals("abcdef")).Select(u => u.FirstName).AsAsyncEnumerable()) {
        Console.WriteLine(name);
    }
}