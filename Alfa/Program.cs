using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Alfa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Declaration
            /*
            List<Subject> subjects = new List<Subject>(Subject.LoadFromFile("../../classes.json"));
            List<Schedule> rated = new List<Schedule>();
            */
            string error = "";
            string filePath = "";
            int timeout = 1;
            int threads = 2;
            
            // User insert
            while (true)
            {
                Console.Clear();
                if (error != "") Console.WriteLine(error);
                Console.Write("Subjects file path [JSON]: ");
                try
                {
                    filePath = Convert.ToString(Console.ReadLine());
                    filePath = Path.GetFullPath(filePath);
                    if (filePath.EndsWith(".json") && Path.IsPathRooted(filePath)) { break;}
                    else throw new Exception();
                } catch (Exception e) { error = "Invalid file path!"; }
            }
            error = "";
            
            List<Subject> subjects = new List<Subject>(Subject.LoadFromFile(filePath));
            List<Schedule> rated = new List<Schedule>();
            
            while (true)
            {
                Console.Clear();
                if (error != "") Console.WriteLine(error);
                try
                {
                    Console.Write($"Threads [2-{Environment.ProcessorCount}]: ");

                    threads = Convert.ToInt32(Console.ReadLine());
                    if (threads >= 2 && threads <= Environment.ProcessorCount) break;
                    else throw new Exception();
                }
                catch (Exception e) { error = "Invalid number of threads!"; }
            }
            error = "";
            
            while (true)
            {
                Console.Clear();
                if (error != "") Console.WriteLine(error);
                Console.Write("Timer [10-100k (s)]: ");
                try
                {
                   timeout = Convert.ToInt32(Console.ReadLine());
                   if (timeout >= 1 && timeout <= 100000) break;
                   else throw new Exception();
                } catch (Exception e) { error = "Invalid time limit!"; }
            }
            
            // Countdown
            Countdown(timeout);
            
            // Generate
            var scheduleGenerator = new ScheduleGenerator(rated, subjects, threads, timeout);
            scheduleGenerator.Generate();
            
            rated = rated.OrderByDescending(obj => obj.Rating).ToList();

            
            // Printing
            Printer.PrintSchedules(rated); 
            
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
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