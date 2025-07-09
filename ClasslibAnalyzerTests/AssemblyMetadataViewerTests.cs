using System;
using System.IO;
using System.Reflection;
using Xunit;
using System.Text;
using System.Linq;
using AssemblyMetadataViewer;

namespace AssemblyMetadataViewer.Tests
{
    public class AssemblyMetadataViewerTests
    {
        private string GetTestAssemblyPath()
        {
            var solutionDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            if (string.IsNullOrEmpty(solutionDir))
            {
                throw new DirectoryNotFoundException("Не удалось найти корневую директорию решения");
            }

            return Path.Combine(solutionDir,"../", "TestClassesLib", "TestClasses.dll");
        }

        private string RunProgramAndGetOutput(string assemblyPath)
        {
            var originalConsoleOut = Console.Out;
            try
            {
                using (var writer = new StringWriter())
                {
                    Console.SetOut(writer);
                    ClassLibraryAnalyzer.PrintAssemblyMetadata(assemblyPath);
                    return writer.ToString().Trim();
                }
            }
            finally
            {
                Console.SetOut(originalConsoleOut);
            }
        }

        [Fact]
        public void PrintAssemblyMetadata_ShouldOutputCorrectAssemblyName()
        {
            string assemblyPath = GetTestAssemblyPath();

            string output = RunProgramAndGetOutput(assemblyPath);

            Assert.Contains("Сборка: TestClasses", output);
        }

        [Fact]
        public void PrintAssemblyMetadata_ShouldOutputAllClasses()
        {
            string assemblyPath = GetTestAssemblyPath();

            string output = RunProgramAndGetOutput(assemblyPath);

            Assert.Contains("Класс: TestClasses.TestClass1", output);
            Assert.Contains("Класс: TestClasses.TestClass2", output);
        }

        [Fact]
        public void PrintTypeMetadata_ShouldOutputClassAttributes()
        {
            string assemblyPath = GetTestAssemblyPath();
            string output = RunProgramAndGetOutput(assemblyPath);
            Assert.Contains("Атрибуты:", output);
            Assert.Contains("DisplayNameAttribute", output);
            Assert.Contains("VersionAttribute", output);
        }

        [Fact]
        public void PrintTypeMetadata_ShouldOutputConstructors()
        {
            string assemblyPath = GetTestAssemblyPath();

            string output = RunProgramAndGetOutput(assemblyPath);

            Assert.Contains("Конструкторы:", output);
            Assert.Contains("TestClass1()", output);
            Assert.Contains("TestClass2()", output);
        }

        [Fact]
        public void PrintTypeMetadata_ShouldOutputMethods()
        {
            string assemblyPath = GetTestAssemblyPath();
            string output = RunProgramAndGetOutput(assemblyPath);

            Assert.Contains("Методы:", output);
            Assert.Contains("  Void TestMethod1()", output);
            Assert.Contains("  Int32 TestMethod2(String input, )", output);

        }

        [Fact]
        public void PrintTypeMetadata_ShouldOutputPropertiesAsMethods()
        {
            string assemblyPath = GetTestAssemblyPath();
            string output = RunProgramAndGetOutput(assemblyPath);

            Assert.Contains("    Int32 get_Number()", output);
            Assert.Contains("    Void set_Number(Int32 value, )", output);
            Assert.Contains("    String get_String()", output);
            Assert.Contains("    Void set_String(String value, )", output);
        }
    }
}
