using System;
using System.ComponentModel.DataAnnotations;

namespace DDoSDetector.Models
{
    public class Network : IDetectionTarget
    {
        public int? Id { get; set; }
        [Required]
        public string Ip { get; set; }
        [Required]
        public string BinaryIp { get; set; }
        [Required]
        public int? NetMaskLength { get; set; }
        public NetworkState State { get; set; }
        public string BlockingReason { get; set; }
        public bool IsSuspicious { get; set; }
        public string SuspicionReason { get; set; }
        public DateTime BlockingTime { get; set; }
    }
}