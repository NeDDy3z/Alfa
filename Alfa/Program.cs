using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Alfa
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<List<List<Subject>>> allSchedules = new List<List<List<Subject>>>();
            List<Subject> subjects = new List<Subject>();
            ScheduleGenerator scheduleGenerator = new ScheduleGenerator(subjects, allSchedules, 50);
            
            Subject.LoadFromFile(subjects, "../../classes.json");
            scheduleGenerator.Generate();
            scheduleGenerator.PrintSchedules();
        }
    }
}