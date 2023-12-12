using System.Collections.Generic;

namespace Alfa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Declaration
            List<Schedule> unratedSchedules = new List<Schedule>();
            List<Schedule> ratedSchedules = new List<Schedule>();
            List<Subject> subjects = new List<Subject>();
            
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator(subjects, unratedSchedules, 1); // in sec
            //ScheduleEvaluator scheduleEvaluator = new ScheduleEvaluator(unratedSchedules, ratedSchedules);
            
            Subject.LoadFromFile(subjects, "../../classes_test.json");
            scheduleGenerator.Generate();
            //scheduleEvaluator.Evaluate();
            
            Printer.PrintStats(unratedSchedules, ratedSchedules); 
            //Printer.PrintSchedules(unratedSchedules);


        }
    }
}