using Attributes;

namespace TestClasses;

[DisplayName("Пример класса для тестирования")]
[Version(1, 0)]
public class TestClass1
{
    [DisplayName("Тестовый конструктор")]
    public TestClass1() { }

    [DisplayName("Тестовый метод 1")]
    public void TestMethod1() { }

    [DisplayName("Тестовый метод 2")]
    public int TestMethod2(string input)
    {
        return input.Length;
    }

    [DisplayName("Числовое свойство")]
    public int Number { get; set; }
}

[DisplayName("Пример класса для тестирования")]
[Version(1, 2)]
public class TestClass2
{
    [DisplayName("Тестовый конструктор")]
    public TestClass2() { }

    [DisplayName("Тестовый метод 1")]
    public void TestMethod1() { }

    [DisplayName("Тестовый метод 2")]
    public int TestMethod2(string input)
    {
        return input.Length;
    }

    [DisplayName("Строчное свойство")]
    public string? String { get; set; }

}
