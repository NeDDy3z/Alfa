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

        public Generator(List<Schedule> generatedSchedules, List<Subject> subjects)
        {
            this._generatedSchedules = generatedSchedules;
            this._subjects = subjects;
        }
        
        public void GenerateSchedules(bool invert)
        {
            var allPermutations = GetPermutations(_subjects);
            if (invert) allPermutations.Reverse();
            
            foreach (var permutation in allPermutations)
            {
                Schedule schedule = new Schedule();
                
                for (int i = 0; i < 5; i++) schedule.Scheduledays.Add(permutation.Skip(i * 10).Take(10).ToList());
                
                if (CheckForDuplicates(schedule)) _generatedSchedules.Add(schedule);
            }
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
        
        public IEnumerable<IEnumerable<T>> GetPermutations<T>(List<T> list)
        {
            if (list.Count == 1)
                return new List<List<T>> { list };

            return list.SelectMany(
                (e, i) => GetPermutations(list.Take(i).Concat(list.Skip(i + 1)).ToList()),
                (e, c) => c.Prepend(e)
            );
        }
    }
}