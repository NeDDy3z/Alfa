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
        private Generator _generator;
        private Evaluator _evaluator;
        private int _threads;
        private int _timeout;
        
        public ScheduleGenerator(List<Schedule> ratedSchedules, List<Subject> subjects, int threads,int timeout)
        {
            this._generatedSchedules = new List<Schedule>();
            this._ratedSchedules = ratedSchedules;
            this._generator = new Generator(_generatedSchedules, subjects);
            this._evaluator = new Evaluator();
            this._threads = threads;
            this._timeout = timeout;
        }
        
        public void Generate()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            List<Task> tasks = new List<Task>();
            int t1, t2;
            if (_threads % 2 == 0)
            {
                t1 = _threads / 2; 
                t2 = _threads / 2;
            }
            else
            {
                t1 = _threads / 2 + 1;
                t2 = _threads / 2;
            }
            for (int i = 0; i < t1; i++)
            {
                tasks.Add(Task.Run(() => _generator.GenerateSchedules(token)));
            }
            for (int i = 0; i < t2; i++)
            {
                tasks.Add(Task.Run(() => Evaluate(token)));
            }

            Task.WaitAll(tasks.ToArray(), TimeSpan.FromSeconds(_timeout)); 
        }

        
        private void Evaluate(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) break;
                try {
                    Schedule schedule = _generatedSchedules[_generatedSchedules.Count - 1];
                    schedule = _evaluator.EvaluateSchedule(schedule);
                    if (schedule.Rating > 0) _ratedSchedules.Add(schedule);
                    
                    if (_ratedSchedules.Count > 3)
                    {
                        _ratedSchedules = _ratedSchedules.OrderBy(obj => obj.Rating).ToList();
                        _ratedSchedules.RemoveRange(5, _ratedSchedules.Count - 5);
                    }
                }
                catch (ArgumentOutOfRangeException) { }
            }
        }
    }
}