using Interfaces;
using task11;

namespace task11tests;

public class CodeCompilerTests
{
     [Fact]
    public void CodeCompilerTests_ReturnCorrectInstance()
    {
        string code = @"
            public class Calculator : Interfaces.ICalculator
            {
                public int Add(int a, int b) => a + b;
                public int Minus(int a, int b) => a - b;
                public int Mul(int a, int b) => a * b;
                public int Div(int a, int b) => a / b;
            }";

        ICalculator instance = CodeCompiler.CodeGenerate(code);

        Assert.NotNull(instance);
        Assert.Equal(5, instance.Add(2, 3));
        Assert.Equal(-1, instance.Minus(2, 3));
        Assert.Equal(6, instance.Mul(2, 3));
        Assert.Equal(2, instance.Div(6, 3));
    }
}
