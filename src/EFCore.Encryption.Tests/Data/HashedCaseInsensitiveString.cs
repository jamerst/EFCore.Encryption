namespace EFCore.Encryption.Tests.Data;

public class HashedCaseInsensitiveString : HashedString
{
    public HashedCaseInsensitiveString() : base() { }
    public HashedCaseInsensitiveString(string value) : base(value) { }

    public override string TransformValue(string value)
    {
        return value.ToLower();
    }

    public static implicit operator HashedCaseInsensitiveString(string value) => new HashedCaseInsensitiveString(value);
}