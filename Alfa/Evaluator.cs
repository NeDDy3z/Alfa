using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Alfa
{
    public class Evaluator
    {
        public Schedule EvaluateSchedule(Schedule schedule)
        {
            int totalPoints = 0;
            for (int dayIndex = 0; dayIndex < schedule.Scheduledays.Count; dayIndex++)
            {
                List<Subject> day = schedule.Scheduledays[dayIndex];

                // 1. If first hour is empty add points
                if (day[0].SubjectName == "") totalPoints += 100;
                // 1.2. if last 4 hours are empty add points
                if (day.Skip(6).Take(4).All(subject => subject.SubjectName == "") ||
                    day.Skip(7).Take(3).All(subject => subject.SubjectName == ""))
                    totalPoints += 100;
                else totalPoints -= 100;

                // 2. Check for duplicate & thrupliacte subjects w/ theory:true
                if (day.Any(subject =>
                        subject.SubjectName != "" && subject.Theory &&
                        day.Count(s => s.SubjectName == subject.SubjectName) >= 3))
                    totalPoints -= 50;
                else if (day.Any(subject =>
                             subject.SubjectName != "" && subject.Theory &&
                             day.Count(s => s.SubjectName == subject.SubjectName) > 1))
                    if (day.Zip(day.Skip(1), (s1, s2) => s1.SubjectName == s2.SubjectName).Any(isEqual => isEqual))
                        totalPoints += 20;
                    else totalPoints -= 50;
                
                
                // 3. Check for different floor levels
                if (day.Any(subject => subject.SubjectName != "" && subject.Floor == 4))
                    totalPoints += 10;
                else totalPoints -= 10;

                // 4. Check for null (free time) subjects at specific positions
                if (!Enumerable.Range(4, 4).Any(position => day[position].SubjectName == ""))
                    totalPoints += 10;
                else totalPoints -= 100;

                // 5. Check number of subjects per day
                if (day.Count(subject => subject.SubjectName != "") <= 6)
                    totalPoints += 50;
                else totalPoints -= 10;

                // 6. Check for practical subjects
                if (day.Any(subject =>
                        subject.SubjectName != "" && day.Count(s => s.SubjectName == subject.SubjectName) > 1))
                    if (day.Zip(day.Skip(1), (s1, s2) => s1.SubjectName == s2.SubjectName).Any(isEqual => isEqual))
                        totalPoints += 10;
                    else totalPoints -= 50;

                // 7. Check if A, C, AM, M arent first or last 4 subjects of the day
                List<String> profileSubjects = new List<string>() { "C", "AM", "M" };
                if (!(profileSubjects.Any(s => s == day[0].SubjectName) || day.Skip(6).Any(subject => profileSubjects.Any(s => s == subject.SubjectName))))
                    totalPoints += 50;
                else totalPoints -= 50;
                
                // 8. Check if practical subjects are together
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
                                else if (j != 0 && !(schedule.Scheduledays[i][j].SubjectName ==
                                                     schedule.Scheduledays[i][j - 1].SubjectName &&
                                                     schedule.Scheduledays[i][j - 1].Theory == false))
                                    totalPoints -= 1000;
                                else totalPoints += 50;
                            }
                            else if (!(schedule.Scheduledays[i][j].SubjectName ==
                                       schedule.Scheduledays[i][j - 1].SubjectName &&
                                       schedule.Scheduledays[i][j - 1].Theory == false)) totalPoints -= 1000;
                            else totalPoints += 50;
                        }
                    }
                }

                // 9 TV on the end = bonus points
                // If subject with subjectname="TV" is last subject of the day add 100 points
                if (day[5].SubjectName == "TV" || day[6].SubjectName == "TV") totalPoints += 100;
                
                
                // 10. When I meet Pustomas I want to kill myself -> (-50p, fuck you, bastard.)
                if (day.Any(subject => subject.Teacher != "" && day.Count(s => s.Teacher == "Masopust") >= 3))
                    totalPoints += 20;
                else totalPoints -= 10;
                
                
            }

            schedule.Rating = totalPoints;
            return schedule;
        }
    }
}