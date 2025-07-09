namespace Attributes;

[AttributeUsage(
    AttributeTargets.All,
    Inherited = false,
    AllowMultiple = true
)]
public sealed class DisplayNameAttribute : Attribute
{
    public string DisplayName { get; }

    public DisplayNameAttribute(string displayName)
    {
        DisplayName = displayName;
    }
}
