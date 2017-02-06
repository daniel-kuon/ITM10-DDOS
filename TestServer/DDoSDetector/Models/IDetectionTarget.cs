using System;

namespace DDoSDetector.Models
{
    public interface IDetectionTarget
    {
        DateTime BlockingTime { get; set; }
        string BlockingReason { get; set; }
        
    }
}