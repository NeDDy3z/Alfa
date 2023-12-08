using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alfa
{
    public class ScheduleGenerator
    {
        private List<List<List<Subject>>> allSchedules;
        private List<Subject> subjects;
        private Stopwatch stopwatch;
        private int timeout;
        
        public ScheduleGenerator(List<Subject> subjects, List<List<List<Subject>>> allSchedules, int timeout)
        {
            this.subjects = subjects;
            this.allSchedules = allSchedules;
            this.timeout = timeout;
            this.stopwatch = new Stopwatch();
        }

        
        public void Generate()
        {
            stopwatch.Start();

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Use Task.Run to run the GenerateSchedules method asynchronously
            var task = Task.Run(() => GenerateSchedules(cancellationToken), cancellationToken);

            // Wait for the task to complete or for the specified time
            if (!task.Wait(TimeSpan.FromMilliseconds(timeout)))
            {
                Console.WriteLine("Generation timed out. Terminating...");
                cancellationTokenSource.Cancel();
            }
        }
        
        private void GenerateSchedules(CancellationToken cancellationToken)
        {
            var permutations = GetPermutations(subjects);

            // Distribute subjects across days
            foreach (var permutation in permutations)
            {
                // Check if cancellation is requested
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("Generation canceled.");
                    break;
                }

                List<List<Subject>> schedule = new List<List<Subject>>();

                for (int i = 0; i < 5; i++)
                {
                    List<Subject> daySubjects = permutation.Skip(i * 10).Take(10).ToList();
                    schedule.Add(daySubjects);
                }
                allSchedules.Add(schedule);
            }
        }
        
        
        private IEnumerable<IEnumerable<T>> GetPermutations<T>(List<T> list)
        {
            if (list.Count == 1)
                return new List<List<T>> { list };

            return list.SelectMany(
                (e, i) => GetPermutations(list.Take(i).Concat(list.Skip(i + 1)).ToList()),
                (e, c) => c.Prepend(e)
            );
        }
        
        public void PrintSchedules()
        {
            Console.WriteLine("Variants generated: "+ allSchedules.Count);
            int scheduleNumber = 1;
            foreach (var schedule in allSchedules)
            {
                Console.WriteLine($"Schedule {scheduleNumber++}:"); 
                for (int i = 0; i < schedule.Count; i++)
                {   
                    Console.WriteLine($"Day {i + 1}: {string.Join(", ", schedule[i].Select(subject => subject.ToString()))}");
                }

                Console.WriteLine();
            }
        }
    }
}