using System;
using System.Collections.Generic;
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
        private int _timeout;
        
        public ScheduleGenerator(List<Subject> subjects, List<Schedule> ratedSchedules, int timeout)
        {
            this._generatedSchedules = new List<Schedule>();
            this._ratedSchedules = ratedSchedules;
            this._generator = new Generator(subjects, _generatedSchedules);
            this._evaluator = new Evaluator();
            this._timeout = timeout;
        }

        public void Generate()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var task = Task.Run(() => GenerateAndEvaluate(cancellationToken), cancellationToken);

            if (!task.Wait(TimeSpan.FromSeconds(_timeout))) cancellationTokenSource.Cancel();
        }

        private void GenerateAndEvaluate(CancellationToken cancellationToken)
        {
            // run generator in another thread
            Task.Run(() => _generator.GenerateSchedules(cancellationToken, 1), cancellationToken);
            /*
            Task.Run(() => _generator.GenerateSchedules(cancellationToken, 2), cancellationToken);
            Task.Run(() => _generator.GenerateSchedules(cancellationToken, 3), cancellationToken);
            Task.Run(() => _generator.GenerateSchedules(cancellationToken, 4), cancellationToken);
            */
            //Evaluate the schedules
            while (true)
            {
                try
                {
                    Schedule schedule = _generatedSchedules[0];
                    _generatedSchedules.RemoveAt(0);
                
                    _ratedSchedules.Add(_evaluator.EvaluateSchedule(schedule));
                    if (_ratedSchedules.Count >= 3)
                    {
                        _ratedSchedules = _ratedSchedules.OrderBy(obj => obj.Rating).ToList();
                        _ratedSchedules.RemoveRange(3, _ratedSchedules.Count - 3);
                    } 
                }
                catch (ArgumentOutOfRangeException) {} 
            }
        }
    }
}