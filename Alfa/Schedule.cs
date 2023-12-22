using System.Collections.Generic;

namespace Alfa
{
    /// <summary>
    /// Represents a schedule with a rating and a list of scheduled subjects per day.
    /// </summary>
    public class Schedule
    {
        private int _rating;
        private List<List<Subject>> _schedule;

        /// <summary>
        /// Initializes a new instance of the <see cref="Schedule"/> class with default values.
        /// </summary>
        public Schedule()
        {
            this._rating = 0;
            this._schedule = new List<List<Subject>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Schedule"/> class with specified rating and schedule.
        /// </summary>
        /// <param name="rating">The rating assigned to the schedule.</param>
        /// <param name="schedule">The list of scheduled subjects per day.</param>
        public Schedule(int rating, List<List<Subject>> schedule)
        {
            this._rating = rating;
            this._schedule = schedule;
        }

        /// <summary>
        /// Gets or sets the rating assigned to the schedule.
        /// </summary>
        public int Rating
        {
            get { return this._rating; }
            set { this._rating = value; }
        }

        /// <summary>
        /// Gets or sets the list of scheduled subjects per day.
        /// </summary>
        public List<List<Subject>> Scheduledays
        {
            get { return this._schedule; }
            set { this._schedule = value; }
        }
    }
}