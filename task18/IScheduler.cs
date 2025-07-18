namespace task18;

public interface IScheduler
{
    bool HasCommand { get; }
    ICommand Select();
    void Add(ICommand cmd);
}
