using System;
using DDoSDetector.Models;

namespace DDoSDetector.Multipliers
{
    public class ResponseSizeCostMultiplier : ICostMultiplier
    {
        private readonly Func<long, double> _calculation;

        public ResponseSizeCostMultiplier(Func<long, double> calculation)
        {
            _calculation = calculation;
        }

        public ResponseSizeCostMultiplier(double multiplier)
        {
            _calculation = d => d * multiplier;
        }

        public ResponseSizeCostMultiplier(long averageSize, double multiplier)
        {
            _calculation = d => (d / (double) averageSize) * multiplier;
        }

        public ResponseSizeCostMultiplier(long averageSize)
        {
            _calculation = d => d / (double) averageSize;
        }

        public double Calculate(Request request)
        {
            return _calculation(request.HttpRequest.HttpContext.Response.ContentLength ?? 0);
        }
    }
}