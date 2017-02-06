using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;

namespace DDoSDetector.Models
{
    public class ClientIp : IDetectionTarget
    {
        public int Id { get; set; }
        [Required]
        public string BinaryIp { get; set; }
        public Network CurrentNetwork { get; set; }
        public int CurrentNetworkId { get; set; }
        public string BlockingReason { get; set; }
        public bool IsSuspicious { get; set; }
        public string SuspicionReason { get; set; }
        public ObservableCollection<Request> Requests { get; set; }=new ObservableCollection<Request>();
        [NotMapped]
        public bool NewEntry { get; set; } = true;

        public int CurrentRequestCosts { get; set; }
        

        public bool IsWhiteListed { get; set; }
        public bool IsBlackListed { get; set; }
        
        public IPAddress Ip { get; set; }
        public bool IsIpV6 { get; set; }
        public DateTime BlockingTime { get; set; }
        

        public ClientIp()
        {
           // Requests.CollectionChanged+=RequestCollectionChanged;
        }

    }
}