using System.Collections.Generic;

namespace Alfa
{
    public class Schedule
    {
        private int _rating;
        private List<List<Subject>> _schedule;

        public Schedule()
        {
            this._rating = 0;
            this._schedule = new List<List<Subject>>();
        }
        public Schedule(int rating, List<List<Subject>> schedule)
        {
            this._rating = rating;
            this._schedule = schedule;
        }   
        

        public int Rating { get { return this._rating; } set { this._rating = value; } }
        public List<List<Subject>> Scheduledays { get { return this._schedule; } set { this._schedule = value; } }
    }
}