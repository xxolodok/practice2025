using Itask04;
namespace task04;

public class Cruiser : ISpaceship
{
    public int Speed { get; }
    public int FirePower { get; }

    public Cruiser()
    {
        Speed = 50;
        FirePower = 100;
    }
    public void Fire()
    {
        Console.WriteLine($"{this.GetType().Name} стреляет!");
    }

    public void MoveForward()
    {
        Console.WriteLine($"{this.GetType().Name} движется вперед со скоростью {Speed}!");
    }

    public void Rotate(int angle)
    {
        Console.WriteLine($"{this.GetType().Name} поворачивается на угол {angle}!");
    }
}

public class Fighter : ISpaceship
{
    public int Speed { get; }

    public int FirePower { get; }

    public Fighter()
    {
        Speed = 100;
        FirePower = 50;
    }
    public void Fire()
    {
        Console.WriteLine($"{this.GetType().Name} стреляет!");
    }
    public void MoveForward()
    {
        Console.WriteLine($"{this.GetType().Name} движется вперед со скоростью {Speed}!");
    }
    public void Rotate(int angle)
    {
        Console.WriteLine($"{this.GetType().Name} поворачивается на угол {angle}!");
    }
}
