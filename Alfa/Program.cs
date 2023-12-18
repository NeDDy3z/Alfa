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
            string filePath = "../../classes.json";
            int timeout = 10;
            
            bool ok = true;
            while (ok)
            {
                Console.Write("Timer [s]: ");
                timeout = Convert.ToInt32(Console.ReadLine());
                if (timeout >= 1 && timeout <= 100000) ok = false;
            }

            
            
            
            // Timer
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
            

            
            // Generate
            var scheduleGenerator = new ScheduleGenerator(Subject.LoadFromFile(filePath), timeout);
            scheduleGenerator.Generate();
            
            // Printing
            Printer.PrintSchedules(scheduleGenerator.RatedSchedules);
        }
    }
}