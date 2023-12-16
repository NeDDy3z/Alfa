using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace Alfa
{
    public class ScheduleGenerator
    {
        private List<Schedule> _generatedSchedules;
        private List<Schedule> _ratedSchedules;
        private List<Subject> _subjects;
        private Generator _generator;
        private Evaluator _evaluator;
        private int _timeout;

        public ScheduleGenerator(List<Subject> subjects, int timeout)
        {
            this._generatedSchedules = new List<Schedule>();
            this._ratedSchedules = new List<Schedule>();
            this._subjects = subjects;
            this._generator = new Generator(_generatedSchedules, _subjects);
            this._evaluator = new Evaluator();
            this._timeout = timeout;
        }

        public List<Schedule> GeneratedSchedules { get { return this._generatedSchedules; } }
        public List<Schedule> RatedSchedules { get { return this._ratedSchedules; } }
        
        public void Generate()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            
            // Create two threads for generating schedules
            Task t1 = Task.Run(() => _generator.GenerateSchedules(false));
            Task t2 = Task.Run(() => _generator.GenerateSchedules(true));
            Task t3 = Task.Run(() => Evaluate(token));

            Task.WaitAll(new Task[] { t1, t2, t3 }, TimeSpan.FromSeconds(_timeout));
        }

        
        private void Evaluate(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) break;
                try {
                    Schedule schedule = _generatedSchedules[_generatedSchedules.Count - 1];
                    _ratedSchedules.Add(_evaluator.EvaluateSchedule(schedule));
                    if (_ratedSchedules.Count >= 5)
                    {
                        _ratedSchedules = _ratedSchedules.OrderByDescending(obj => obj.Rating).ToList();
                        _ratedSchedules.RemoveRange(5, _ratedSchedules.Count - 5);
                    }
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }
    }
}