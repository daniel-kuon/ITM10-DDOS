using System;
using DDoSDetector.Models;

namespace DDoSDetector.Multipliers
{
    public class ResponseDurationCostMultiplier : ICostMultiplier
    {
        private readonly Func<int, double> _calculation;

        public ResponseDurationCostMultiplier(Func<int, double> calculation)
        {
            _calculation = calculation;
        }

        public ResponseDurationCostMultiplier(double multiplier)
        {
            _calculation = d => d * multiplier;
        }

        public ResponseDurationCostMultiplier(int averageTime)
        {
            _calculation = d => d / (double) averageTime;
        }

        public ResponseDurationCostMultiplier(double multiplier, int averageTime)
        {
            _calculation = d => (d / (double) averageTime) * multiplier;
        }

        public double Calculate(Request request)
        {
            return _calculation(DateTime.Now.Subtract(request.Time).Milliseconds);
        }
    }
}