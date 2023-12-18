using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alfa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Declaration
            List<Subject> subjects = new List<Subject>(Subject.LoadFromFile("../../classes_test.json"));
            List<Schedule> rated = new List<Schedule>();
            int timeout;
            
            // User insert
            while (true)
            {
                //Console.Clear();
                Console.Write("Timer [s]: ");
                timeout = Convert.ToInt32(Console.ReadLine());
                if (timeout >= 1 && timeout <= 100000) break;
            }
            
            // Countdown
            //Countdown(timeout);
            
            // Generate
            var scheduleGenerator = new ScheduleGenerator(rated, subjects, timeout);
            scheduleGenerator.Generate();
            
            // Printing
            Console.WriteLine(rated.Count);
            Printer.PrintSchedules(rated); 
            
        }

        static void Countdown(int timeout)
        {
            Task.Run(() =>
                {
                    string start = DateTime.Now.ToString("HH:mm:ss");
                    string end = DateTime.Now.AddSeconds(timeout).ToString("HH:mm:ss");
                    while (timeout > 0)
                    {
                        Console.Clear();
                        Console.WriteLine(start +" - "+ end +"\n"+timeout--);
                        Thread.Sleep(1000);
                    }
                }
            );
        }
    }
}