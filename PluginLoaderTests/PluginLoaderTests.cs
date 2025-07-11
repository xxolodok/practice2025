using System;
using System.IO;
using Xunit;
using PluginLoader;

namespace PluginLoaderTests
{
    public class PluginLoaderTests
    {
        [Fact]
        public void LoadAndExecutePlugins_ExecutesPluginsInCorrectOrder()
        {
            //Arrange
            var output = new StringWriter();
            Console.SetOut(output);

            var testProjectPath = Directory.GetCurrentDirectory();
            string solutionPath = Path.Combine(testProjectPath, "..", "..", "..", "..");
            solutionPath = Path.GetFullPath(solutionPath);
            string pluginsPath = $"{solutionPath}/PluginLib/";

            // Act
            PluginLoader.PluginLoader.LoadAndExecutePlugins(pluginsPath);

            // Assert
            var consoleOutput = output.ToString();
            var indexOfPluginOne = consoleOutput.IndexOf("PluginOne is executing!");
            var indexOfPluginTwo = consoleOutput.IndexOf("PluginTwo is executing!");

            Assert.True(indexOfPluginOne >= 0, "PluginOne не был выполнен");
            Assert.True(indexOfPluginTwo >= 0, "PluginTwo не был выполнен");
            Assert.True(indexOfPluginOne < indexOfPluginTwo,
                "PluginOne должен выполняться перед PluginTwo");

        }
    }
}