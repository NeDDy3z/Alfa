using System.Collections.Generic;

namespace Alfa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Declaration
            List<List<List<Subject>>> unratedSchedules = new List<List<List<Subject>>>();
            List<List<List<Subject>>> ratedSchedules = new List<List<List<Subject>>>();
            List<Subject> subjects = new List<Subject>();
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator(subjects, unratedSchedules, 300);
            Printer printer = new Printer();
            
            
            
            Subject.LoadFromFile(subjects, "../../classes.json");
            scheduleGenerator.Generate();
            printer.PrintSchedules(unratedSchedules);
            
            
        }
    }
}