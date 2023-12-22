using System;
using System.Collections.Generic;
using System.Linq;

namespace Alfa
{
    /// <summary>
    /// Evaluates the quality of a schedule based on various criteria.
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// Evaluates the given schedule and assigns a rating based on predefined criteria.
        /// </summary>
        /// <param name="schedule">The schedule to be evaluated.</param>
        /// <returns>The schedule with an assigned rating.</returns>
        public Schedule EvaluateSchedule(Schedule schedule)
        {
            int totalPoints = 0;
            for (int subjectIndex = 0; subjectIndex < schedule.Scheduledays.Count; subjectIndex++)
            {
                List<Subject> day = schedule.Scheduledays[subjectIndex];
                int subjectsCount = day.Count(subject => subject.SubjectName != "");


                // 1. First hour is empty add points
                if (day[0].SubjectName == "") totalPoints += 100;

                // 1.1 Monday or Frirday have 4-6 hours
                if (subjectIndex == 0 || subjectIndex == 4 && subjectsCount <= 6 && subjectsCount >= 4)
                    totalPoints += 1000;
                else totalPoints -= 100;

                // 1.2 Last 4 hours are empty add points
                if (day.Skip(6).Take(4).All(subject => subject.SubjectName == ""))
                    totalPoints += 100;
                else totalPoints -= 100;


                // 2. Check for duplicate & thrupliacte subjects w/ theory:true
                if (day.Any(subject => subject.Theory && day.Count(s => s.SubjectName == subject.SubjectName) > 1 &&
                                       subject.SubjectName == day[day.IndexOf(subject) + 1].SubjectName))
                    //if (day.Zip(day.Skip(1), (s1, s2) => s1.SubjectName == s2.SubjectName).Any(isEqual => isEqual)) totalPoints += 20;
                    totalPoints += 100;
                else totalPoints -= 1000;


                // 3. Check for different floor levels
                if (day.Any(subject => subject.SubjectName != "" && subject.Floor == 4))
                    totalPoints += 100;
                else totalPoints -= 100;


                // 4. Check for (free time) subjects at specific positions
                if (!Enumerable.Range(4, 4).Any(position => day[position].SubjectName == ""))
                    totalPoints += 100;
                else totalPoints -= 100;


                // 5. Check number of subjects per day
                if (day.Count(subject => subject.SubjectName != "") <= 7)
                    totalPoints += 1000;
                else totalPoints -= 100;


                // 6. Practical subjects are grouped together
                if (day.Any(subject =>
                        subject.SubjectName != "" && day.Count(s => s.SubjectName == subject.SubjectName) > 1))
                    if (day.Zip(day.Skip(1), (s1, s2) => s1.SubjectName == s2.SubjectName).Any(isEqual => isEqual))
                        totalPoints += 100;
                    else totalPoints -= 1000;


                // 7. C, AM, M arent first or last subject of the day
                List<String> profileSubjects = new List<string>() { "C", "AM", "M" };
                if (profileSubjects.Any(s => s != day[0].SubjectName) || day.Skip(subjectsCount - 2)
                        .Any(subject => profileSubjects.Any(s => s != subject.SubjectName)))
                    totalPoints += 100;
                else totalPoints -= 100;


                // 8. Check if identical subjects are together
                for (int i = 0; i < day.Count - 2; i++)
                {
                    if (!day[i].Theory)
                    {
                        if (day[i].SubjectName == day[i + 1].SubjectName)
                        {
                            totalPoints += 100;
                            i++;
                        }
                        else totalPoints -= 100_000;
                    }
                    else if (day[i].Theory && (day.Count(subject => subject.SubjectName == day[i].SubjectName) >= 2))
                    {
                        if (day[i].SubjectName == day[i + 1].SubjectName)
                        {
                            totalPoints += 100;
                            i++;
                        }
                        else totalPoints -= 100_000;
                    }
                }

                // 8.1 check for long days
                if (day.Count > 10) totalPoints -= 100_000;
                else totalPoints += 1000;


                // 9. TV is the last subject of the day (bcs im sweaty :3)
                if (day.Any(s =>
                        s.SubjectName == "TV" &&
                        day.Where(subject => subject.SubjectName != "").ToList()[subjectsCount - 1].SubjectName ==
                        "TV"))
                    totalPoints += 5000;
                else totalPoints -= 1000;


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