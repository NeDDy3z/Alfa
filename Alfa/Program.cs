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
            Printer printer = new Printer(unratedSchedules, ratedSchedules);
            
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator(subjects, unratedSchedules, 100); // in sec
            
            Subject.LoadFromFile(subjects, "../../classes_test.json");
            scheduleGenerator.Generate();
            printer.PrintStats();
            //printer.PrintSchedules();


        }
    }
}