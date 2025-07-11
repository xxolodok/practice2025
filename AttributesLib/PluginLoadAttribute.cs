using System;

namespace AttributesLib;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class PluginLoadAttribute : Attribute
{
    public Type[] Dependencies { get; }

    public PluginLoadAttribute(params Type[] dependencies)
    {
        Dependencies = dependencies ?? Array.Empty<Type>();
    }
}
