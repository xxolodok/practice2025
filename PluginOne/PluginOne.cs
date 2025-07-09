using AttributesLib;
using Interfaces;

namespace PluginOne;

[PluginLoad]
public class PluginOne : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("PluginOne is executing!");
    }
}
