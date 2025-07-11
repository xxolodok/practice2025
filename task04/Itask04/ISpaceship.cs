namespace Itask04;

public interface ISpaceship
{
    int Speed { get; }
    int FirePower { get; }
    void MoveForward();
    void Rotate(int angle);
    void Fire();
}
