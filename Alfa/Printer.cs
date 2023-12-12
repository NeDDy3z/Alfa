using System;
using System.Collections.Generic;
using System.Linq;

namespace Alfa
{
    public class Printer
    {
        private static Dictionary<int, string> _days = new Dictionary<int, string>()
        {
            { 1, "Po" },
            { 2, "Út" },
            { 3, "St" },
            { 4, "Čt" },
            { 5 ,"Pá" }
        };
        
        public static void PrintSchedules(List<Schedule> schedules)
        {
            int scheduleNumber = 1;
            foreach (var schedule in schedules)
            {
                Console.Write($"\n#{scheduleNumber++} "); 
                PrintSchedule(schedule);
            }
        }

        public static void PrintSchedule(Schedule schedule)
        {
            Console.Write($"[{schedule.Rating}]");
            Console.WriteLine("\n       1.    2.    3.    4.    5.    6.    7.    8.    9.    10.");
            string temp = "";
            for (int i = 0; i < schedule.Scheduledays.Count; i++)
            {
                temp = "";
                for (int j = 0; j < schedule.Scheduledays[i].Count; j++)
                {
                    if (schedule.Scheduledays[i][j].SubjectName.Length == 1) temp += schedule.Scheduledays[i][j].SubjectName + "   |";
                    else if (schedule.Scheduledays[i][j].SubjectName.Length == 2) temp += schedule.Scheduledays[i][j].SubjectName + "  |";
                    else if (schedule.Scheduledays[i][j].SubjectName.Length == 3) temp += schedule.Scheduledays[i][j].SubjectName + " |";
                    else temp += "    |";
                    temp += " ";
                }
                Console.WriteLine($"{_days[i+1]}: | {temp}");
            }
            Console.WriteLine();
        }

        public static void PrintStats(List<Schedule> unratedSchedules, List<Schedule> ratedSchedules)
        {
            Console.WriteLine($"Generated: {unratedSchedules.Count}");
            Console.WriteLine($"Rated: {ratedSchedules.Count}");
        }
    }
}