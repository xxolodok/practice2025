namespace Attributes;

[AttributeUsage(
    AttributeTargets.All,
    Inherited = false,
    AllowMultiple = true
)]
public sealed class VersionAttribute : Attribute
{
    public int Major { get; }
    public int Minor { get; }

    public VersionAttribute(int Major, int Minor)
    {
        this.Major = Major;
        this.Minor = Minor;
    }
}
