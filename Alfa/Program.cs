using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Alfa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine(Convert.ToInt32(Regex.Match(null, @"\d+").Value));
            
            // Declaration
            List<Schedule> finishedSchedules = new List<Schedule>();
            string filePath = "../../classes_test.json";
            
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator(Subject.LoadFromFile(filePath), finishedSchedules, 1000);
            scheduleGenerator.Generate();
            
            Printer.PrintStats(finishedSchedules, finishedSchedules);
            Printer.PrintSchedules(finishedSchedules);
        }
    }
}