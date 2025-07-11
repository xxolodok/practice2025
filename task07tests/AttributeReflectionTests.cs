namespace task07tests;

using System.Reflection;
using Xunit;
using SampleClass;
using Attributes;
using task07;

public class AttributeReflectionTests
{
    [Fact]
    public void Class_HasDisplayNameAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Пример класса", attribute.DisplayName);
    }

    [Fact]
    public void Method_HasDisplayNameAttribute()
    {
        var method = typeof(SampleClass).GetMethod("TestMethod");
        var attribute = method.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Тестовый метод", attribute.DisplayName);
    }

    [Fact]
    public void Property_HasDisplayNameAttribute()
    {
        var prop = typeof(SampleClass).GetProperty("Number");
        var attribute = prop.GetCustomAttribute<DisplayNameAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal("Числовое свойство", attribute.DisplayName);
    }

    [Fact]
    public void Class_HasVersionAttribute()
    {
        var type = typeof(SampleClass);
        var attribute = type.GetCustomAttribute<VersionAttribute>();
        Assert.NotNull(attribute);
        Assert.Equal(1, attribute.Major);
        Assert.Equal(0, attribute.Minor);
    }

    [Fact]
        public void PrintTypeInfo_ShouldOutputCorrectTypeInformation()
        {
            var sampleType = typeof(SampleClass);
            var expectedOutputParts = new[]
            {
                "Отображаемое имя класса: Пример класса",
                "Версия класса: 1.0",
                "Методы:",
                "  TestMethod: Тестовый метод",
                "Свойства:",
                "  Number: Числовое свойство"
            };

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            ReflectionHelper.PrintTypeInfo(sampleType);
            var output = stringWriter.ToString();

            foreach (var expectedPart in expectedOutputParts)
            {
                Assert.Contains(expectedPart, output);
            }
        }
}
