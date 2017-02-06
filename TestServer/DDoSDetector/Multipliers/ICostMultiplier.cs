using DDoSDetector.Models;

namespace DDoSDetector.Multipliers
{
    public interface ICostMultiplier
    {
        double Calculate(Request request);
    }
}