using System;
using System.Collections.Generic;
using System.Linq;

namespace Alfa
{
    public class Printer
    {
        public void PrintSchedules(List<List<List<Subject>>> schedules)
        {
            Dictionary<int, string> days = new Dictionary<int, string>()
            {
                { 1, "Po" },
                { 2, "Út" },
                { 3, "St" },
                { 4, "Čt" },
                { 5 ,"Pá" }
            };
            int scheduleNumber = 1;
            foreach (var schedule in schedules)
            {
                Console.WriteLine($"\nSchedule {scheduleNumber++}:"); 
                Console.WriteLine("    1. 2. 3. 4. 5. 6. 7. 8. 9. 10.");
                string temp = "";
                for (int i = 0; i < schedule.Count; i++)
                {
                    temp = "";
                    for (int j = 0; j < schedule[i].Count; j++)
                    {
                        if (schedule[i][j].SubjectName.Length == 1) temp += schedule[i][j].SubjectName + "  ";
                        if (schedule[i][j].SubjectName.Length == 2) temp += schedule[i][j].SubjectName + " ";
                    }
                    //Console.WriteLine($"{days[i+1]}: {string.Join(" ", schedule[i].Select(subject => subject.SubjectName))}");
                    Console.WriteLine($"{days[i+1]}: {temp}");
                }

            }
        }
    }
}