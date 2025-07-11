namespace Attributes;

[System.AttributeUsage(
    System.AttributeTargets.All,
    Inherited = false,
    AllowMultiple = true
)]
public sealed class VersionAttribute : System.Attribute
{
    public int Major { get; }
    public int Minor { get; }

    public VersionAttribute(int Major, int Minor)
    {
        this.Major = Major;
        this.Minor = Minor;
    }
}
