using task17;
using Interfaces;
using System;
using Xunit;
using System.Threading;

namespace task17tests;

public class ServerThreadTests
{
    [Fact]
    public void HardStop_StopsThreadInstantly()
    {
        var serverThread = new ServerThread();
        serverThread.Start();

        bool command1Executed = false;
        bool command2Executed = false;

        serverThread.EnqueueCommand(new TestCommand(() => command1Executed = true));
        serverThread.EnqueueCommand(new HardStop(serverThread));
        serverThread.EnqueueCommand(new TestCommand(() => command2Executed = true));

        Thread.Sleep(100); 

        Assert.True(command1Executed);
        Assert.False(command2Executed);
        Assert.False(serverThread.Thread.IsAlive);
    }

    [Fact]
    public void HardStop_ThrowsWhenExecutedInWrongThread()
    {
        var serverThread = new ServerThread();
        var hardStop = new HardStop(serverThread);

        Assert.Throws<InvalidOperationException>(() => hardStop.Execute());
    }

    [Fact]
    public void SoftStop_ThrowsWhenExecutedInWrongThread()
    {
        var serverThread = new ServerThread();
        var softStop = new SoftStop(serverThread);

        Assert.Throws<InvalidOperationException>(() => softStop.Execute());
    }

    [Fact]
    public void Thread_WaitsForCommandsWithoutCpuUsage()
    {
        var serverThread = new ServerThread();
        serverThread.Start();

        Thread.Sleep(100);

        Assert.True(serverThread.Thread.IsAlive);

        serverThread.EnqueueCommand(new HardStop(serverThread));
        Thread.Sleep(100);
        Assert.False(serverThread.Thread.IsAlive);
    }

    private class TestCommand : ICommand
    {
        private readonly Action _action;

        public TestCommand(Action action)
        {
            _action = action;
        }

        public void Execute()
        {
            _action?.Invoke();
        }
    }
}
