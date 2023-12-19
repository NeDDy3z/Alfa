using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Alfa
{
    public class Generator
    {
        private List<Subject> _subjects;
        private List<Schedule> _generatedSchedules;
        private int _generatedCount;
        private Random _random;

        public Generator(List<Subject> subjects, List<Schedule> generatedSchedules)
        {
            this._subjects = subjects;
            this._generatedSchedules = generatedSchedules;
            this._generatedCount = 0;
            this._random = new Random();
        }
        
        public int GeneratedCount => _generatedCount;
        
        public void GenerateSchedules(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) break;
                
                List<Subject> randomSubjects = new List<Subject>();
                randomSubjects.AddRange(Randomize(_subjects));
                Schedule schedule = new Schedule();

                //while (randomSubjects.Count != 34) Thread.Sleep(500);
                
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
                _generatedCount += 1;
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
                if (index % 6 == 0) index += 2;
                subjects.InsertRange(index, practicalSubjects.GetRange(i,2));
            }
            
            return subjects;
        }
    }
}