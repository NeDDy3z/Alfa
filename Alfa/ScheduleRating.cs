using System;
using System.Collections.Generic;
using System.Linq;

namespace Alfa
{
    public class ScheduleRating
    {
        public int EvaluateSchedule(List<List<List<Subject>>> schedule)
        {
            int totalPoints = 0;

            for (int dayIndex = 0; dayIndex < schedule.Count; dayIndex++)
            {
                List<Subject> day = schedule[dayIndex].SelectMany(subjects => subjects).ToList();

                // 1. Each hour has its own rating
                if (day[0].SubjectName == null) totalPoints += 50_000;
                if (day.Take(5).Any(subject => subject.SubjectName == null)) totalPoints += 50_000;

                // 2. Check for duplicate subjects
                if (day.Any(subject => subject.SubjectName != null && subject.Theory && day.Count(s => s.SubjectName == subject.SubjectName) == 1))
                    totalPoints += 1_000_000;
                else totalPoints -= 10_000_000;

                // 3. Check classroom number (different floor levels)
                if (day.Any(subject => subject.SubjectName != null && Convert.ToInt32(subject.Classroom) < 20))
                    totalPoints += 200_000;
                else totalPoints -= 200_000;

                // 4. Check for null (free time) subjects at specific positions
                if (!Enumerable.Range(4, 6).Any(position => day[position].SubjectName == null))
                    totalPoints += 100_000;
                else totalPoints -= 10_000_000;

                // 5. Check number of subjects per day
                if (day.Count(subject => subject.SubjectName != null) <= 6)
                    totalPoints += 5_000_000;
                else totalPoints -= 100_000;

                // 6. Check for adjacent duplicate subjects
                for (int i = 0; i < day.Count; i++)
                {
                    if (day[i].SubjectName == day[i + 1].SubjectName && !day[i].Theory) totalPoints += 100_000;
                    else totalPoints -= 10_000_000;
                }
                
                // 7. Check if A, C, AM, M arent first or last 5 subjects of the day
                List<String> profileSubjects = new List<string>() { "A", "C", "AM", "M" };
                if (day.Take(5).Any(subject =>
                        subject != null && profileSubjects.Any(item => item != subject.SubjectName)) &&
                        profileSubjects.Any(subject => subject != day[0].SubjectName))
                    totalPoints += 500_000;
                else totalPoints -= 50_000;
            }

            return totalPoints;
        }


    }
}