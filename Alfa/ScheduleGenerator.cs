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
        private List<List<List<Subject>>> unratedSchedules;
        private List<Subject> subjects;
        private Stopwatch stopwatch;
        private int timeout;


        public ScheduleGenerator(List<Subject> subjects, List<List<List<Subject>>> unratedSchedules, int timeout)
        {
            this.subjects = subjects;
            this.unratedSchedules = unratedSchedules;
            this.timeout = timeout;
            this.stopwatch = new Stopwatch();
        }

        public void Generate()
        {
            stopwatch.Start();

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var task = Task.Run(() => GenerateSchedules(cancellationToken), cancellationToken);

            if (!task.Wait(TimeSpan.FromSeconds(timeout)))
            {
                Console.WriteLine("Generation timed out. Terminating...");
                cancellationTokenSource.Cancel();
            }
        }

        private void GenerateSchedules(CancellationToken cancellationToken)
        {
            var permutations = GetPermutations(subjects);

            foreach (var permutation in permutations)
            {
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
                if (IsValidSchedule(schedule)) unratedSchedules.Add(schedule);
            }
        }

        private bool IsValidSchedule(List<List<Subject>> schedule)
        {
            // If you find subject with theory == false, there must be another subject with same name and theory == false next to it, if there is, skip the next iteration, otherwise return false
            for (int i = 0; i < schedule.Count; i++)
            {
                for (int j = 0; j < schedule[i].Count; j++)
                {
                    if (!schedule[i][j].Theory)
                    {
                        if (j == 0)
                        {
                            if (schedule[i][j].SubjectName == schedule[i][j + 1].SubjectName &&
                                schedule[i][j + 1].Theory == false) continue;
                            else return false;
                        }
                        else if (j == 9)
                        {
                            if (schedule[i][j].SubjectName == schedule[i][j - 1].SubjectName &&
                                schedule[i][j - 1].Theory == false) continue;
                            else return false;
                        }
                        else
                        {
                            if (schedule[i][j].SubjectName == schedule[i][j + 1].SubjectName &&
                                schedule[i][j + 1].Theory == false) continue;
                            else if (schedule[i][j].SubjectName == schedule[i][j - 1].SubjectName &&
                                     schedule[i][j - 1].Theory == false) continue;
                            else return false;
                        }
                    }
                }
            }
            
            // Compare to the previous schedule and if it's the same, return false
            if (unratedSchedules.Count > 0)
            {
                foreach (var unratedSchedule in unratedSchedules)
                {
                    if (schedule.SequenceEqual(unratedSchedule)) return false;
                }
            }


            return true;
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
    }
}