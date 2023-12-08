using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alfa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<List<List<string>>> allSchedules = new List<List<List<string>>>();
            List<string> subjects = new List<string>
            {
                "M", "DS", "DS", "PSS", "PSS", "A", null, "TV", null, null,
                "PIS", "M", "PIS", "PIS", "TP", "A", "CJ", null, null, null,
                "CIT", "CIT", "WA", "DS", "PV", null, "PSS", null, null, null,
                "AM", "M", "WA", "WA", null, "A", "C", "PIS", "TV", null,
                "C",   "A", "M", "PV", "PV", "AM", null, null, null, null 
            };
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator(subjects, allSchedules);

            scheduleGenerator.Generate();
            PrintSchedules(allSchedules);
            
                
            
        }
        
        static void PrintSchedules(List<List<List<string>>> schedules)
        {
            int scheduleNumber = 1;

            foreach (var schedule in schedules)
            {
                Console.WriteLine($"Schedule {scheduleNumber++}:");

                for (int i = 0; i < schedule.Count; i++)
                {
                    Console.WriteLine($"Day {i + 1}: {string.Join(", ", schedule[i])}");
                }

                Console.WriteLine();
            }
        }
    }
}