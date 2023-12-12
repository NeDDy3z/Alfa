using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Alfa
{
    public class Evaluator
    {   
        public Schedule EvaluateSchedule(Schedule schedule)
        {
            int totalPoints = 0;

            for (int dayIndex = 0; dayIndex < schedule.Scheduledays.Count(); dayIndex++)
            {
                List<Subject> day = schedule.Scheduledays[dayIndex];

                // 1. If first hour is empty || last 4 are empty +50k points
                if (day[0].SubjectName == null || day.Take(4).Any(subject => subject.SubjectName == null))
                    totalPoints += 1000;

                // 2. Check for duplicate subjects
                if (day.Any(subject => subject.SubjectName != null && subject.Theory && day.Count(s => s.SubjectName == subject.SubjectName) == 1))
                    totalPoints += 1000;
                else totalPoints -= 100;

                // 3. Check classroom number (different floor levels)
                /*
                if (day.Any(subject => subject.SubjectName != null && Convert.ToInt32(Regex.Match(subject.Classroom, @"\d+").Value) < 20))
                    totalPoints += 200_000;
                else totalPoints -= 200_000;
                */

                // 4. Check for null (free time) subjects at specific positions
                if (!Enumerable.Range(4, 6).Any(position => day[position].SubjectName == null))
                    totalPoints += 1000;
                else totalPoints -= 100;

                // 5. Check number of subjects per day
                if (day.Count(subject => subject.SubjectName != null) <= 6)
                    totalPoints += 2500;
                else totalPoints -= 200;

                // 6. Check for adjacent duplicate subjects
                for (int i = 0; i < day.Count; i++)
                {
                    if (i < 9)
                    {
                        if (day[i].SubjectName == day[i + 1].SubjectName && !day[i].Theory) totalPoints += 1000;
                        else totalPoints -= 1000;   
                    }
                }
                
                // 7. Check if A, C, AM, M arent first or last 5 subjects of the day
                List<String> profileSubjects = new List<string>() { "A", "C", "AM", "M" };
                if (day.Take(5).Any(subject =>
                        subject != null && profileSubjects.Any(item => item != subject.SubjectName)) &&
                        profileSubjects.Any(subject => subject != day[0].SubjectName))
                    totalPoints += 5000;
                else totalPoints -= 200;
                
                //8.
                //9.
                //10. When I meet Pustomas I want to kill myself -> -50k
                if (day.Any(subject => subject.Teacher.Contains("Masopust")))
                    totalPoints -= 100;
            }

            schedule.Rating = totalPoints;
            return schedule;
        }
    }
}