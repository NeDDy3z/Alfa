using System.Collections.Generic;

namespace Alfa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Declaration
            List<Schedule> finishedSchedules = new List<Schedule>();
            string filePath = "../../classes_test.json";
            
            // Generation
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator(Subject.LoadFromFile(filePath), finishedSchedules, 10);
            scheduleGenerator.Generate();
            
            // Printing
            Printer.PrintSchedules(finishedSchedules);
        }
    }
}