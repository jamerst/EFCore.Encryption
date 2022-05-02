using EFCore.Encryption.Tests.Data;

namespace EFCore.Encryption.Tests.Models;

public class User
{
    public int Id { get; set; }
    public EncryptedName FirstName { get; set; } = null!;
    public EncryptedName? MiddleName { get; set; }
    public EncryptedName Surname { get; set; } = null!;
    public EncryptedDateOnly DateOfBirth { get; set; } = null!;
    public EncryptedString SomeExternalIdentifier { get; set; } = null!;

    public string ToOutputString() => $"{SomeExternalIdentifier}: {FirstName} {MiddleName} {Surname}, born {DateOfBirth:d}";
}