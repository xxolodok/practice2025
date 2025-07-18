namespace task18;

public class RoundRobinScheduler : IScheduler
{
    private readonly Queue<ICommand> _commands = new Queue<ICommand>();
    private readonly object _lock = new object();

    public bool HasCommand { get { lock (_lock) return _commands.Count > 0; } }

    public void Add(ICommand cmd)
    {
        if (cmd == null) throw new ArgumentNullException(nameof(cmd));
        lock (_lock) _commands.Enqueue(cmd);
    }

    public ICommand Select()
    {
        lock (_lock)
        {
            if (_commands.Count == 0) return null;

            var cmd = _commands.Dequeue();
            if (!cmd.IsCompleted)
            {
                _commands.Enqueue(cmd);
            }
            return cmd;
        }
    }
}
