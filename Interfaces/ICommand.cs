namespace Interfaces;

using System;
using System.Collections.Concurrent;
using System.Threading;

public interface ICommand
{
    void Execute();
}
