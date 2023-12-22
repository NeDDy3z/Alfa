using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        private Stopwatch _stopwatch;
        
        public int GeneratedCount => _generator.GeneratedCount;

        public ScheduleGenerator(List<Schedule> ratedSchedules, List<Subject> subjects, int threads, int timeout)
        {
            this._generatedSchedules = new List<Schedule>();
            this._ratedSchedules = ratedSchedules;
            this._generator = new Generator(subjects, _generatedSchedules);
            this._evaluator = new Evaluator();
            this._threads = threads;
            this._timeout = timeout;
            this._stopwatch = new Stopwatch();
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
            
            for (int i = 0; i < t1; i++) tasks.Add(new Task(() => _generator.GenerateSchedules(token)));
            for (int i = 0; i < t2; i++) tasks.Add(new Task(() => Evaluate(token)));
            foreach (var task in tasks) task.Start();
            
            Task.WaitAll(tasks.ToArray(), TimeSpan.FromSeconds(_timeout));
            cts.Cancel();
        }
        
        private void Evaluate(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested) break;
                try {
                    Schedule schedule = _generatedSchedules[_generatedSchedules.Count - 1];
                    _generatedSchedules.RemoveAt(_generatedSchedules.IndexOf(schedule));

                    schedule = _evaluator.EvaluateSchedule(schedule);
                    
                    if (schedule.Rating > -100_000) _ratedSchedules.Add(schedule);
                    if (_ratedSchedules.Count > 5)
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