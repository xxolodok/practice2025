namespace task17;

using Interfaces;
using System.Collections.Concurrent;

public class ServerThread : IDisposable
{
    private readonly BlockingCollection<ICommand> _commandQueue = new BlockingCollection<ICommand>();
    private readonly Thread _thread;
    private volatile bool _isRunning;
    private volatile bool _stopRequested;
    public Thread Thread => _thread;

    public ServerThread()
    {
        _thread = new Thread(Run);
        _isRunning = false;
    }

    public void Start()
    {
        if (_isRunning) return;
        
        _isRunning = true;
        _stopRequested = false;
        _thread.Start();
    }

    public void EnqueueCommand(ICommand command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        _commandQueue.Add(command);
    }

    private void Run()
    {
        while (_isRunning)
        {
            try
            {
                if (_commandQueue.TryTake(out var command, Timeout.Infinite))
                {
                    command.Execute();

                    if (_stopRequested && _commandQueue.Count == 0)
                    {
                        StopInstantly();
                    }
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Console.WriteLine($"Исключение: {ex}");
            }
        }
    }

    internal void StopInstantly()
    {
        _isRunning = false;
        _commandQueue.CompleteAdding();
    }

    internal void StopGracefully()
    {
        _stopRequested = true;
        if (_commandQueue.Count == 0)
        {
            StopInstantly();
        }
    }

    public void Dispose()
    {
        StopInstantly();
        _thread.Join();
        _commandQueue.Dispose();
    }
}

public class HardStop : ICommand
{
    private readonly ServerThread _serverThread;

    public HardStop(ServerThread serverThread)
    {
        _serverThread = serverThread ?? throw new ArgumentNullException(nameof(serverThread));
    }

    public void Execute()
    {
        if (Thread.CurrentThread != _serverThread.Thread)
        {
            throw new InvalidOperationException("HardStop работает только в целевом потоке");
        }
        _serverThread.StopInstantly();
    }
}

public class SoftStop : ICommand
{
    private readonly ServerThread _serverThread;

    public SoftStop(ServerThread serverThread)
    {
        _serverThread = serverThread ?? throw new ArgumentNullException(nameof(serverThread));
    }

    public void Execute()
    {
        if (Thread.CurrentThread != _serverThread.Thread)
        {
            throw new InvalidOperationException("SoftStop работает только в целевом потоке");
        }
        _serverThread.StopGracefully();
    }
}
