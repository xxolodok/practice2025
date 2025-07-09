using AttributesLib;
using Interfaces;
using PluginOne;
namespace PluginTwo;

[PluginLoad(typeof(PluginOne.PluginOne))]
public class PluginTwo : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("PluginTwo is executing!");
    }
}
