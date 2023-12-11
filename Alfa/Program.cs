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
            
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator(subjects, unratedSchedules, 10); // in sec
            //ScheduleEvaluator scheduleEvaluator = new ScheduleEvaluator(unratedSchedules, ratedSchedules);
            
            Subject.LoadFromFile(subjects, "../../classes_test.json");
            scheduleGenerator.Generate();
            //scheduleEvaluator.Evaluate();
            
            Printer.PrintStats(unratedSchedules, ratedSchedules);
            //printer.PrintSchedules();


        }
    }
}