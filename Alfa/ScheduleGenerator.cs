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
                
                if (IsValidSchedule(schedule))
                {
                    Console.WriteLine(schedule);
                    unratedSchedules.Add(schedule);
                }
            }
        }

        private bool IsValidSchedule(List<List<Subject>> schedule)
        {
            // Check if subjects with the same name and theory:false are together
            for (int i = 0; i < schedule.Count; i++)
            {
                for (int j = 0; i < schedule[i].Count - 1; j++)
                {
                    if (schedule[i][j].SubjectName == schedule[i][j + 1].SubjectName && !schedule[i][j].Theory) j++;
                    else return false;
                }
                
                // Check if there are more than one subjects with the same name and theory:true per day
                var theorySubjects = schedule[i].Where(subject => subject.Theory).ToList();
                var theorySubjectNames = theorySubjects.Select(subject => subject.SubjectName).ToList();
                if (theorySubjectNames.Distinct().Count() != theorySubjectNames.Count()) return false;
            }

            /*            
            // Check if only two different subjects with different names were switched
            var subjectNames = new HashSet<string>();
            for (int day = 0; day < 5; day++)
            {
                var daySubjects = schedule[day];
                foreach (var subject in daySubjects)
                {
                    if (subjectNames.Contains(subject.SubjectName)) return false; // Subjects with the same name were switched
                    subjectNames.Add(subject.SubjectName);
                }
            }
            */
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