namespace Attributes;

[System.AttributeUsage(
    System.AttributeTargets.All,
    Inherited = false,
    AllowMultiple = true
)]
public sealed class DisplayNameAttribute : System.Attribute
{
    public string DisplayName { get; }

    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}
