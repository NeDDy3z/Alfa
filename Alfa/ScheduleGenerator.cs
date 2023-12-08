using System;
using System.Collections.Generic;
using System.Linq;

namespace Alfa
{
    public class ScheduleGenerator
    {
        private List<string> subjects;
        private List<List<List<string>>> allSchedules;
        
        public ScheduleGenerator(List<string> subjects, List<List<List<string>>> allSchedules)
        {
            this.subjects = subjects;
            this.allSchedules = allSchedules;
        }


        
        public void Generate()
        {
            var permutations = GetPermutations(subjects);
            
            // Distribute subjects across days
            foreach (var permutation in permutations)
            {
                List<List<string>> schedule = new List<List<string>>();

                for (int i = 0; i < 5; i++)
                {
                    // Each day can have up to 10 subjects
                    List<string> daySubjects = permutation.Skip(i * 10).Take(10).ToList();
                    schedule.Add(daySubjects);
                }

                allSchedules.Add(schedule);
                //SchedulePrinter(schedule);
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

        private void SchedulePrinter(List<List<string>> schedule)
        {
            Console.WriteLine("Schedule");
            foreach (var day in schedule)
            {
                for (int i = 0; i < schedule.Count; i++)
                {
                    Console.WriteLine($"Day {i + 1}: {string.Join(", ", schedule[i])}");
                }
                Console.WriteLine();
            }
        }
    }
}