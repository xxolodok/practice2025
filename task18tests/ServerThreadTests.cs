namespace task18tests;

using Xunit;
using System;
using System.Threading;
using System.Collections.Generic;
using task18;


public class ServerThreadTests
{
    [Fact]
    public void LongRunningCommand_ExecutesInChunks()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);
        server.Start();

        var cmd = new LongRunningCommand();
        server.EnqueueCommand(cmd);

        Thread.Sleep(3000);

        Assert.True(cmd.IsCompleted);
        server.Dispose();
    }

    [Fact]
    public void MultipleCommands_ExecuteInterleaved()
    {
        var scheduler = new RoundRobinScheduler();
        var server = new ServerThread(scheduler);
        server.Start();

        var results = new List<int>();
        var cmd1 = new IterativeCommand(3, () => results.Add(1));
        var cmd2 = new IterativeCommand(3, () => results.Add(2));

        server.EnqueueCommand(cmd1);
        server.EnqueueCommand(cmd2);

        Thread.Sleep(1000);
        server.Dispose();

        Assert.Contains(1, results);
        Assert.Contains(2, results);
        Assert.Equal(8, results.Count);
    }

    private class LongRunningCommand : ICommand
    {
        private int _remainingIterations = 10;

        public bool IsCompleted => _remainingIterations <= 0;

        public void Execute()
        {
            if (IsCompleted) return;
            Thread.Sleep(50);
            _remainingIterations--;
        }
    }

    private class IterativeCommand : ICommand
    {
        private int _iterations;
        private readonly Action _action;

        public bool IsCompleted => _iterations <= 0;

        public IterativeCommand(int iterations, Action action)
        {
            _iterations = iterations;
            _action = action;
        }

        public void Execute()
        {
            _action();
            _iterations--;
        }
    }
}
