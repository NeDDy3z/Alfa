using System.Collections.Generic;

namespace Alfa
{
    public class Schedule
    {
        private int _rating;
        private List<List<Subject>> _schedule;

        public Schedule(List<List<Subject>> schedule)
        {
            this._rating = 0;
            this._schedule = schedule;
        }

        public int Rating { get { return this._rating; } set { this._rating = value; } }
        public List<List<Subject>> Schedules { get { return this._schedule; } set { this._schedule = value; } }
    }
}