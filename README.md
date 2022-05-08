# EFCore.Encryption
Column-level encryption for Entity Framework Core.

EFCore.Encryption allows you to more easily store and manage sensitive data with Entity Framework Core.

A common problem with many column-level encryption implementations is that they:
- Create lots of repetitive code to encrypt/decrypt values
- Prevent querying due to non-deterministic encryption

EFCore.Encryption provides a helpful base for building your own encrypted column implementations of any type. It also supports querying for equality by storing a hashed copy of the data alongside the encrypted data, which is of course deterministic.

EFCore.Encryption should be compatible with any SQL database provider (tested with Postgres, Sqlite, SQL Server and MySQL).

Note that EFCore.Encryption is not a full implementation, you will need to implement some parts yourself such as the encryption and hashing.

## Getting Started
To get started, simply install the `EFCore.Encryption` NuGet package.

EFCore.Encryption has only two public classes: `EncryptedFieldBase` and `HashedValueBase`. Implementing these abstract classes (or implementing your own versions of `IEncryptedField` and `IHashedValue`) is all that's required to start storing your data.

EFCore.Encryption is designed to allow multiple implementations of these classes so that you can store data of different types or semantics.

Since some methods will be the same for these implementations I recommend that you create your own base classes (like `EncryptedField` and `HashedValue` in the sample implementation) to contain these duplicate methods.

### Sample
There is a sample application included in this repository which provides examples of how to implement and use the classes. Feel free to use the implementations from there in your own code.

### HashedValueBase
HashedValueBase is the base of any type used to store the hashed version of an encrypted column. This class is a wrapper around a simple string column to store the hash.

This class is generic, the generic argument is the data type you are storing.

#### Implementing
The following methods should be implemented:
- `ComputeHash(byte[] bytes)` and `ComputeHashAsync(byte[] bytes)` - compute the hash of a given byte array, return as a string (e.g. base64 encoded)
- `ToBinary(T value)` - convert a value into a byte array

The `GetHashValue` method is optional. This method allows you to modify the value before it is hashed, e.g. to allow case-insensitive querying you should normalise the case of the string value.

Note that you will need to add the parameterless constructor since constructors are not inherited in C#.

If implementing your own `IHashedValue` you will need to add the `[NotParameterized]` attribute to the parameter of the `HashEquals` method, otherwise the translation will fail.

#### Registering
Before using your implementation you need to register it with EFCore by calling the `UseHashedType` method:
```
services.AddDbContext<MyContext>(options => options
    .UseDatabaseProvider(...)
    .UseHashedType<HashedString, string>()
    .UseHashedType<HashedCaseInsensitiveString, string>()
    .UseHashedType<HashedDateOnly, DateOnly>()
)
// this can also be done in DbContext.OnConfiguring
```

The `UseHashedType` method also has an overload which allows you to specify the store type (type used in the database for the hashed column). This defaults to `TEXT`, but you may wish to change this to a fixed-length type since the hash will always be the same length.

#### SQL Server Users
**If you are using SQL Server you will need to specify a different type in `UseHashedType` such as `NVARCHAR`.**

This is because SQL Server doesn't just treat the `TEXT` data type as an alias to a regular string type, so this causes issues when comparing values to it.

#### Usage
The only member of this class that you are likely to use is the `HashEquals` method. This compares the hashed value with the computed hash of another value, which allows the aforementioned case-insensitive querying.

This method is special because this library provides the EFCore translation, so it can be used in database queries without being evaluated in-memory!

### EncryptedFieldBase
EncryptedFieldBase is the base data type to be used in your model classes. If you want to store an encrypted value, the property on your model will be some implementation of this class.

This class uses the [owned entity](https://docs.microsoft.com/en-us/ef/core/modeling/owned-entities) feature of EF Core, which allows both the encrypted and hashed columns to be accessed from a single property in C#, and allows the hashed column to be automatically updated whenever the value is updated.

This class has two generic arguments, the type of the value being stored, and the type of the hashed value.

#### Implementing
The following methods should be implemented:
- `Encrypt(byte[] bytes)` and `EncryptAsync(byte[] bytes)` - return the encrypted bytes of a byte array
- `Decrypt(byte[] bytes)` and `DecryptAsync(byte[] bytes)` - return the decrypted bytes of a byte array
- `ToBinary(T value)` - convert a value into a byte array
- `FromBinary(byte[] bytes)` - convert a byte array back into the value

The `Value`, `GetValueAsync` and `SetValueAsync` methods are virtual so you can override these if you wish (e.g. if you wanted to add caching to avoid decrypting the value multiple times, though this may have security issues of course).

I recommend adding implicit casts from the value type to the encrypted type to make it cleaner and easier to create the encrypted values:
```
public static implicit operator EncryptedString(string val) => new EncryptedString(val);

// this then allows something like:
entity.SomeEncryptedString = "Something secret";
```

#### Usage
To use an encrypted field, simply add one as a property in your model:
```
public class User
{
    public int Id { get; set; }
    public EncryptedName FirstName { get; set; } = null!;
    public EncryptedName? MiddleName { get; set; }
    public EncryptedName Surname { get; set; } = null!;
    public EncryptedDateOnly DateOfBirth { get; set; } = null!;
    public EncryptedString SomeExternalIdentifier { get; set; } = null!;
}
```

I strongly recommend using [nullable reference types](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references) to improve the clarity and type-checking of your code.

## Querying
Querying of the encrypted columns is still limited due to the nature of the problem, but it is at least easy and consistent. Simply use the `HashEquals` method like so:

```
db.Users.Where(u => u.Surname.Hashed.HashEquals("smith"));
```

**Note that the value provided in HashEquals must be a constant or variable, it cannot be a reference to another column.**

This is much easier to work with because the hashing of the value is handled internally, so you don't need to remember the different hashing methods for every column, e.g. which columns are case-sensitive and which are not.

## Common Issues & Pitfalls
- Be careful when implementing case-insensitive encrypted fields. The default implementation only updates the encrypted value if the hashed value changes, so if the case of the value is updated this won't be detected and the value won't be changed.
- Whilst this library is tested significant parts are not implemented internally so cannot be tested. Be sure to test your implementations thoroughly.

## Limitations
Whilst the library works well, there are some things that I think could be done better:
- Ideally the HashEquals method would be in EncryptedFieldBase instead to eliminate the need for the custom HashedValue classes. This isn't currently possible as methods on owned entities cannot be translated. This would be possible if [this issue](https://github.com/dotnet/efcore/issues/13947) is resolved in EFCore 7.0 as currently planned.
- I'm not a big fan of the encryption, decryption and hashing methods being part of the class. Ideally these would be provided via a service that can be dependency injected, but I can't think of a way to do this without making it very clunky.
- It would be nice to eliminate the use of the `new()` constraint since it [doesn't perform particularly well](https://devblogs.microsoft.com/premier-developer/dissecting-the-new-constraint-in-c-a-perfect-example-of-a-leaky-abstraction/).