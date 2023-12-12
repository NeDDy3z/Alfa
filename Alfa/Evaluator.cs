using System;
using System.Collections.Generic;
using System.Linq;

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
                    totalPoints += 100;

                // 2. Check for duplicate & thrupliacte subjects w/ theory:true
                if (day.Any(subject =>
                        subject.SubjectName != null && subject.Theory &&
                        day.Count(s => s.SubjectName == subject.SubjectName) >= 3))
                    totalPoints -= 100;
                else if (day.Any(subject => subject.SubjectName != null && subject.Theory && day.Count(s => s.SubjectName == subject.SubjectName) > 1))
                    if (day.Zip(day.Skip(1), (s1, s2) => s1.SubjectName == s2.SubjectName).Any(isEqual => isEqual))
                        totalPoints += 10;
                    else totalPoints -= 100;
                

                // 3. Check classroom number (different floor levels)
                /*
                if (day.Any(subject => subject.SubjectName != null && Convert.ToInt16(subject.Classroom) < 20))
                    totalPoints += 100;
                else totalPoints -= 200;
                */

                // 4. Check for null (free time) subjects at specific positions
                if (!Enumerable.Range(4, 4).Any(position => day[position].SubjectName == null))
                    totalPoints += 50;
                else totalPoints -= 100;
                
                // 5. Check number of subjects per day
                if (day.Count(subject => subject.SubjectName != null) <= 6)
                    totalPoints += 50;
                else totalPoints -= 10;

                // 6. Check for adjacent duplicate subjects
                if (day.Any(subject => subject.SubjectName != null && day.Count(s => s.SubjectName == subject.SubjectName) > 1))
                    if (day.Zip(day.Skip(1), (s1, s2) => s1.SubjectName == s2.SubjectName).Any(isEqual => isEqual))
                        totalPoints += 10;
                    else totalPoints -= 100;
                
                // 7. Check if A, C, AM, M arent first or last 5 subjects of the day
                List<String> profileSubjects = new List<string>() { "C", "AM", "M" };
                if (day.Take(5).Any(subject =>
                        subject != null && profileSubjects.Any(item => item != subject.SubjectName)) &&
                        profileSubjects.Any(subject => subject != day[0].SubjectName))
                    totalPoints += 30;
                else totalPoints -= 10;
                
                //8.
                //if there are only less than 4 subjects (with name not null) in the day, subtract 10000 points
                if (!(day.Count(subject => subject.SubjectName != null) < 4))
                    totalPoints += 100;
                else totalPoints -= 100;
                
                //9.
                //10. When I meet Pustomas I want to kill myself -> (-50p, fuck you, bastard.)
                if (day.Any(subject => subject.Teacher != null && day.Count(s => s.Teacher == "Masopust") >= 3))
                    totalPoints += 20;
                else totalPoints -= 10;
            }

            schedule.Rating = totalPoints;
            return schedule;
        }
    }
}