using System;
using System.Threading;
using System.Linq;

namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function,
                              double step, int threadsNumber)
    {
        double totalResult = 0.0;
        double intervalLength = b - a;
        double subIntervalLength = intervalLength / threadsNumber;

        Thread[] threads = new Thread[threadsNumber];

        var barrier = new Barrier(threadsNumber + 1);
        threads = Enumerable.Range(0, threadsNumber).Select(i =>
        {
            double start = a + i * subIntervalLength;
            double end = (i == threadsNumber - 1) ? b : start + subIntervalLength;

            var thread = new Thread(() =>
            {
                double localResult = 0.0;
                double x = start;

                while (x < end)
                {
                    double nextX = Math.Min(x + step, end);
                    double y1 = function(x);
                    double y2 = function(nextX);

                    localResult += (y1 + y2) * (nextX - x) / 2;
                    x = nextX;
                }

                double current, newValue;
                current = totalResult;
                newValue = current + localResult;
                Interlocked.CompareExchange(ref totalResult, newValue, current);

                barrier.SignalAndWait();
            });

            return thread;
        }).ToArray();

        foreach (var thread in threads)
        {
            thread.Start();
        }

        barrier.SignalAndWait();
        barrier.Disponse();

        return totalResult;
    }
}
