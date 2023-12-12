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
        private List<Schedule> _unratedSchedules;
        private List<Subject> _subjects;
        private Stopwatch _stopwatch;
        private int _timeout;


        public ScheduleGenerator(List<Subject> subjects, List<Schedule> unratedSchedules, int timeout)
        {
            this._subjects = subjects;
            this._unratedSchedules = unratedSchedules;
            this._timeout = timeout;
            this._stopwatch = new Stopwatch();
        }

        public void Generate()
        {
            _stopwatch.Start();

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var task = Task.Run(() => GenerateSchedules(cancellationToken), cancellationToken);

            if (!task.Wait(TimeSpan.FromSeconds(_timeout)))
            {
                Console.WriteLine("Generation timed out. Terminating...");
                cancellationTokenSource.Cancel();
            }
        }

        private void GenerateSchedules(CancellationToken cancellationToken)
        {
            var permutations = GetPermutations(_subjects);

            foreach (var permutation in permutations)
            {
                if (cancellationToken.IsCancellationRequested) break;

                Schedule schedule = new Schedule();
                for (int i = 0; i < 5; i++)
                {
                    List<Subject> daySubjects = permutation.Skip(i * 10).Take(10).ToList();
                    schedule.Scheduledays.Add(daySubjects);
                }
                if (IsValidSchedule(schedule.Scheduledays)) _unratedSchedules.Add(schedule);
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
                        if (j < 9)
                        {
                            if (schedule[i][j].SubjectName == schedule[i][j + 1].SubjectName &&
                                schedule[i][j + 1].Theory == false) continue;
                            else if (!(schedule[i][j].SubjectName == schedule[i][j - 1].SubjectName &&
                                       schedule[i][j - 1].Theory == false)) return false;
                        }
                        else if (j == 9)
                        {
                            if (!(schedule[i][j].SubjectName == schedule[i][j - 1].SubjectName &&
                                schedule[i][j - 1].Theory == false)) return false;
                        }
                    }
                }
            }
            
            // Compare to the previous schedule and if it's the same, return false
            if (_unratedSchedules.Count > 0)
            {
                foreach (var unratedSchedule in _unratedSchedules)
                {
                    if (schedule.SequenceEqual(unratedSchedule.Scheduledays)) return false;
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