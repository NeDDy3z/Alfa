using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Alfa
{
    public class Generator
    {
        private List<Schedule> _generatedSchedules;
        private List<Subject> _subjects;

        public Generator(List<Subject> subjects, List<Schedule> generatedSchedules)
        {
            this._subjects = subjects;
            this._generatedSchedules = generatedSchedules;
        }
        
        public void GenerateSchedules(CancellationToken cancellationToken)
        {
            foreach (var permutation in GetPermutations(_subjects))
            {
                if (cancellationToken.IsCancellationRequested) break;
                Schedule schedule = new Schedule();
                for (int i = 0; i < 5; i++)
                {
                    List<Subject> daySubjects = permutation.Skip(i * 10).Take(10).ToList();
                    schedule.Scheduledays.Add(daySubjects);
                }
                if (IsValidSchedule(schedule)) _generatedSchedules.Add(schedule);
            } 
        }

        private bool IsValidSchedule(Schedule schedule)
        {
            // If you find subject with theory == false, there must be another subject with same name and theory == false next to it, if there is, skip the next iteration, otherwise return false
            for (int i = 0; i < schedule.Scheduledays.Count; i++)
            {
                for (int j = 0; j < schedule.Scheduledays[i].Count; j++)
                {
                    if (!schedule.Scheduledays[i][j].Theory)
                    {
                        if (j < 9)
                        {
                            if (schedule.Scheduledays[i][j].SubjectName == schedule.Scheduledays[i][j + 1].SubjectName &&
                                schedule.Scheduledays[i][j + 1].Theory == false) continue;
                            else if (j != 0 && !(schedule.Scheduledays[i][j].SubjectName == schedule.Scheduledays[i][j - 1].SubjectName &&
                                       schedule.Scheduledays[i][j - 1].Theory == false)) return false;
                        }
                        else if (j == 9)
                        {
                            if (!(schedule.Scheduledays[i][j].SubjectName == schedule.Scheduledays[i][j - 1].SubjectName &&
                                schedule.Scheduledays[i][j - 1].Theory == false)) return false;
                        }
                    }
                }
            }
            // Compare to the previous schedule and if it's the same, return false
            if (_generatedSchedules.Count > 0)
            {
                foreach (var unratedSchedule in _generatedSchedules)
                {
                    if (schedule.Scheduledays.SequenceEqual(unratedSchedule.Scheduledays)) return false;
                }
            }
            return true;
        }


        private IEnumerable<IEnumerable<T>> GetPermutations<T>(List<T> list)
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