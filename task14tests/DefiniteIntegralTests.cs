using Xunit;
using task14;

namespace task14tests;

public class DefiniteIntegralTests
{
    private readonly Func<double, double> _linearFunction = x => x;
    private readonly Func<double, double> _sinFunction = Math.Sin;

    [Fact]
    public void Solve_LinearFunction_SymmetricInterval_ReturnsZero()
    {
        // Arrange
        double a = -1;
        double b = 1;
        double expected = 0;
        double precision = 1e-4;

        // Act
        double result = DefiniteIntegral.Solve(a, b, _linearFunction, 1e-4, 2);

        // Assert
        Assert.Equal(expected, result, precision);
    }

    [Fact]
    public void Solve_SinFunction_SymmetricInterval_ReturnsZero()
    {
        // Arrange
        double a = -1;
        double b = 1;
        double expected = 0;
        double precision = 1e-4;

        // Act
        double result = DefiniteIntegral.Solve(a, b, _sinFunction, 1e-5, 8);

        // Assert
        Assert.Equal(expected, result, precision);
    }

    [Fact]
    public void Solve_LinearFunction_From0To5_Returns12_5()
    {
        // Arrange
        double a = 0;
        double b = 5;
        double expected = 12.5; 
        double precision = 1e-5;

        // Act
        double result = DefiniteIntegral.Solve(a, b, _linearFunction, 1e-6, 8);

        // Assert
        Assert.Equal(expected, result, precision);
    }
}
