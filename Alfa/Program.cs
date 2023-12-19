    using System;
using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
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
            Stopwatch s = new Stopwatch();
            List<Schedule> rated = new List<Schedule>();
            string filePath;
            int threads;
            int timeout;
            
            // UserInput, input filepath of json file, number of threads and timeout for how long the user wants the program to run
            string error = "";
            while (true)
            {
                Console.Clear();
                if (error.Contains("file")) Console.WriteLine(error);
                Console.Write("Subjects file path [JSON]: ");
                try
                {
                    filePath = Convert.ToString(Console.ReadLine());
                    switch (filePath)
                    {
                        default:
                            filePath = Path.GetFullPath(filePath);
                            if (filePath.EndsWith(".json") && Path.IsPathRooted(filePath)) break;
                            throw new Exception();
                        case "DEBUG":
                            filePath = "DEBUG";
                            break;
                        case "/d":
                            filePath = "../../classes.json";
                            break;
                        case "local":
                            filePath = "classes.json";
                            break;
                    }
                    break;
                } catch (Exception e) { error = "Invalid file path!"; }
            }
            
            // Initialize
            List<Subject> subjects = new List<Subject>(Subject.LoadFromFile(filePath));
            
            while (true)
            {
                Console.Clear();
                if (error.Contains("threads")) Console.WriteLine(error);
                try
                {
                    Console.Write($"Threads [2-{Environment.ProcessorCount}]: ");

                    threads = Convert.ToInt32(Console.ReadLine());
                    if (threads >= 2 && threads <= Environment.ProcessorCount) break;
                    else throw new Exception();
                }
                catch (Exception e) { error = "Invalid number of threads!"; }
            }
            
            while (true)
            {
                Console.Clear();
                if (error.Contains("limit")) Console.WriteLine(error);
                Console.Write("Timer [1-1000 s]: ");
                try
                {
                    timeout = Convert.ToInt32(Console.ReadLine());
                    if (timeout >= 1 && timeout <= 1000) break;
                    else throw new Exception();
                } catch (Exception e) { error = "Invalid time limit!"; }
            }

            
            // Start countdown
            Thread countdown = new Thread(() => Countdown(timeout));
            countdown.Start();
            s.Start();
            
            // Generate
            var scheduleGenerator = new ScheduleGenerator(rated, subjects, threads, timeout);
            scheduleGenerator.Generate();
            
            // Abort countdown when generating is done
            countdown.Abort();
            s.Stop();
            
            // Re-sort the finished list
            rated = rated.OrderByDescending(obj => obj.Rating).ToList();
            rated = rated.Take(5).ToList();
            
            // Printing
            Printer.PrintSchedules(rated);
            Printer.PrintStats(scheduleGenerator.GeneratedCount, s.Elapsed);
            
            // Exit-prompt
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
        
        // Simple countdown clock
        static void Countdown(int timeout)
        {
            string start = DateTime.Now.ToString("HH:mm:ss");
            string end = DateTime.Now.AddSeconds(timeout).ToString("HH:mm:ss");
            while (true)
            {
                for (int i = 1; i < 4; i++)
                {
                    Console.Clear();
                    Console.WriteLine($"{start} - {end}\nGenerating"+ new string('.', i));
                    Thread.Sleep(500);
                }
            }
        }
    }
}