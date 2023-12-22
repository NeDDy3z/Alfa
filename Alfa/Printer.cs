using System;
using System.Collections.Generic;
using System.Diagnostics;

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
            { 5, "Pá" }
        };

        public static void PrintSchedules(List<Schedule> schedules)
        {
            int scheduleNumber = 1;
            if (schedules.Count == 0) Console.WriteLine("No schedules found!");
            else
            {
                Console.Clear();
                foreach (var schedule in schedules)
                {
                    Console.Write($"\n#{scheduleNumber++} ");
                    PrintSchedule(schedule);
                }
            }
        }

        public static void PrintSchedule(Schedule schedule)
        {
            Console.Write($"[{schedule.Rating}]");
            Console.WriteLine("\n     1.    2.    3.    4.    5.    6.    7.    8.    9.   10.");
            for (int i = 0; i < schedule.Scheduledays.Count; i++)
            {
                Console.Write(_days[i + 1] + ": ");
                for (int j = 0; j < schedule.Scheduledays[i].Count; j++)
                {
                    if (schedule.Scheduledays[i][j].Theory) Console.ForegroundColor = ConsoleColor.Green;
                    else Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(schedule.Scheduledays[i][j].SubjectName);

                    Console.ForegroundColor = ConsoleColor.White;
                    string temp = "";
                    if (schedule.Scheduledays[i][j].SubjectName.Length == 1) temp += "   | ";
                    else if (schedule.Scheduledays[i][j].SubjectName.Length == 2) temp += "  | ";
                    else if (schedule.Scheduledays[i][j].SubjectName.Length == 3) temp += " | ";
                    else temp += "    | ";
                    Console.Write(temp);
                }

                Console.WriteLine();
            }
        }

        public static void PrintStats(int generatedCount, TimeSpan s)
        {
            // trim s.tostring() so that it has only 8 characters from start
            Console.WriteLine($"\nGenerated: {generatedCount} schedules\nTime elapsed: {s.ToString().Substring(0, 8)}");
        }
    }
}