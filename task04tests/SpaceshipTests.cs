namespace task04tests;

using Xunit;
using task04;
using Itask04;
public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    //Далее идет реализация недостающих тестов

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Fighter_CorrectCallMethod()
    {
        var fighter = new Fighter();
        fighter.Fire();
        fighter.MoveForward();
        fighter.Rotate(12);
    }

    [Fact]
    public void Cruise_CorrectCallMethod()
    {
        var cruise = new Cruiser();
        cruise.Fire();
        cruise.MoveForward();
        cruise.Rotate(12);
    }

}
