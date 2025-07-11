namespace task05tests;

using Xunit;
using task05;
using System.Runtime.Serialization;

public class TestClass
{
    public int PublicField;
    private string? _privateField;
    public int Property { get; set; }

    public void Method() { }
}

[Serializable]
public class AttributedClass { }

public class ClassAnalyzerTests
{
    [Fact]
    public void GetPublicMethods_ReturnsCorrectMethods()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var methods = analyzer.GetPublicMethods();

        Assert.Contains("Method", methods);
    }

    [Fact]
    public void GetAllFields_IncludesPrivateFields()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var fields = analyzer.GetAllFields();

        Assert.Contains("_privateField", fields);
    }

    [Fact]
    public void GetMethodParams_ReturnNullList()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var param = analyzer.GetMethodParams("Method");

        Assert.Empty(param);
    }

    [Fact]
    public void GetProperties_includesProperty()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var prop = analyzer.GetProperties();

        Assert.Contains("Property", prop);
    }

    [Fact]
    public void TestName()
    {
        var analyzer = new ClassAnalyzer(typeof(TestClass));
        var analyzerForAttributedClass = new ClassAnalyzer(typeof(AttributedClass));

        var rezult1 = analyzer.HasAttribute<SerializableAttribute>();
        var rezult2 = analyzerForAttributedClass.HasAttribute<SerializableAttribute>();

        Assert.False(rezult1);
        Assert.True(rezult2);
    }
}
