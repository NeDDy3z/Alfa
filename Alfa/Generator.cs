using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Alfa
{
    public class Generator
    {
        private List<Schedule> _generatedSchedules;
        private List<Subject> _subjects;
        private Random _random;

        public Generator(List<Schedule> generatedSchedules, List<Subject> subjects)
        {
            this._generatedSchedules = generatedSchedules;
            this._subjects = subjects;
            this._random = new Random();
        }
        
        public void GenerateSchedules()
        {
            while (true)
            {
                List<Subject> randomSubjects = new List<Subject>();
                randomSubjects.AddRange(Randomize(_subjects));
                Schedule schedule = new Schedule();

                while (randomSubjects.Count != 34) Thread.Sleep(1000);
                
                for (int i = 0; i < 5; i++)
                {
                    if (randomSubjects.Count > 7)
                    {
                        schedule.Scheduledays.Add(randomSubjects.Take(7).ToList());
                        randomSubjects.RemoveRange(0, 7);
                    }
                    else schedule.Scheduledays.Add(randomSubjects.Take(randomSubjects.Count).ToList());

                    while (schedule.Scheduledays[i].Count < 10) schedule.Scheduledays[i].Add(new Subject());
                }
                _generatedSchedules.Add(schedule);
            }
        }

        private List<Subject> Randomize(List<Subject> subjects)
        {
            subjects = subjects.OrderBy(s => s.SubjectName).ToList();
            List<Subject> theorySubjects = subjects.Where(s => s.Theory).ToList();
            List<Subject> practicalSubjects = subjects.Where(s => !s.Theory).ToList();
            
            theorySubjects = theorySubjects.OrderBy(x => _random.Next()).ToList();

            subjects.Clear();
            subjects.AddRange(theorySubjects);
            
            
            for (int i = 0; i < practicalSubjects.Count; i += 2)
            {
                int random = _random.Next(subjects.Count - 1);
                int index = random - (random % 2);
                subjects.InsertRange(index, practicalSubjects.GetRange(i,2));
            }
            /*
            foreach (var sub in practicalSubjects)
            {
                subjects.InsertRange(_random.Next(subjects.Count - 1), practicalSubjects.GetRange(0,2));
                //practicalSubjects.RemoveRange(0,2);
            }
            */
            
            return subjects;
        }
        
        private bool CheckForDuplicates(Schedule schedule)
        {
            List<String> scheduleSubjectNames = schedule.Scheduledays.SelectMany(x => x.Select(y => y.SubjectName)).ToList();
            foreach (var sch in _generatedSchedules)
            {
                if (scheduleSubjectNames.SequenceEqual(sch.Scheduledays
                        .SelectMany(x => x.Select(y => y.SubjectName)).ToList())) return false;
            }
            return true;
        }
        
    }
}