using System.Collections.Concurrent;

namespace task18;
using System;
using System.Collections.Concurrent;
using System.Threading;

public class ServerThread : IDisposable
{
    private readonly BlockingCollection<ICommand> _newCommands = new BlockingCollection<ICommand>();
    private readonly IScheduler _scheduler;
    private readonly Thread _thread;
    private volatile bool _isRunning;

    public ServerThread(IScheduler scheduler)
    {
        _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        _thread = new Thread(Run);
        _isRunning = false;
    }

    public void Start()
    {
        if (_isRunning) return;
        _isRunning = true;
        _thread.Start();
    }

    public void EnqueueCommand(ICommand command)
    {
        if (command == null) throw new ArgumentNullException(nameof(command));
        _newCommands.Add(command);
    }

    private void Run()
    {
        while (_isRunning)
        {
            try
            {
                if (_newCommands.TryTake(out var newCmd, 100))
                {
                    _scheduler.Add(newCmd);
                }

                if (_scheduler.HasCommand)
                {
                    var cmd = _scheduler.Select();
                    if (cmd != null)
                    {
                        ExecuteCommand(cmd);
                    }
                }
                else if (!_newCommands.Any()) 
                {
                    Thread.Sleep(100);
                }
            }
            catch (OperationCanceledException)
            {
                
                break;
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка вызова команды", ex);
            }
        }
    }

    private void ExecuteCommand(ICommand cmd)
    {
        try
        {
            cmd.Execute();
        }
        catch (Exception ex)
        {
            throw new Exception($"Команда: {cmd.GetType().Name} не выполнена", ex);
        }
    }

    public void StopImmediately()
    {
        _isRunning = false;
        _newCommands.CompleteAdding();
    }

    public void Dispose()
    {
        StopImmediately();
        _thread.Join();
        _newCommands.Dispose();
    }
}
