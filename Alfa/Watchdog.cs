using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Alfa
{
    public class Watchdog
    {
        public void ExecuteWithTimeout<T>(Func<ScheduleGenerator> action, TimeSpan timeout)
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var task = Task.Run(action, cancellationTokenSource.Token);

                if (Task.WaitAny(new[] { task }, timeout) < 0)
                {
                    cancellationTokenSource.Cancel();
                    Console.WriteLine("Terminated.");
                }
            }
        }
    }
}