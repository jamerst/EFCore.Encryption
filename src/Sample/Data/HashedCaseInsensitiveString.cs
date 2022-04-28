namespace Sample.Data;

public class HashedCaseInsensitiveString : HashedString
{
    public HashedCaseInsensitiveString() : base() { }
    public HashedCaseInsensitiveString(string value) : base(value) { }

    public override string GetHashValue(string value)
    {
        return value.ToLower();
    }

    public static implicit operator HashedCaseInsensitiveString(string value) => new HashedCaseInsensitiveString(value);
}