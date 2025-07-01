namespace SampleClass;

using Attributes;

[DisplayName("Пример класса")]
[Version(1, 0)]
public class SampleClass
{
    [DisplayName("Тестовый метод")]
    public void TestMethod() { }
    [DisplayName("Числовое свойство")]
    public int Number { set; get; }
}
