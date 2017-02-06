using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DDoSDetector.Models
{

    public class Request : IDetectionTarget
    {
        private int _costs;

        public Request(HttpRequest httpRequest)
        {
            var context = httpRequest.HttpContext;
            ClientIp = Repository.GetClientIp(context.Connection.RemoteIpAddress);
            Address = httpRequest.Path;
            HttpMethod = httpRequest.Method;
            Time = DateTime.Now;
            HttpRequest = httpRequest;
            ClientIp.Requests.Add(this);
        }


        public int? Id { get; set; }
        public string HttpMethod { get; set; }
        public string Address{ get; set; }
        public DateTime Time { get; set; }
        public TimeSpan? Duration { get; set; }
        public ClientIp ClientIp { get; }
        public int ClientIpId { get; set; }
        public string BlockingReason { get; set; }
        public bool IsSuspicious { get; set; }
        public bool IsBlocked { get; set; }
        public HttpRequest HttpRequest { get; set; }


        public int Costs
        {
            get { return _costs; }
            set
            {
                ClientIp.CurrentRequestCosts -= _costs;
                _costs = value;
                ClientIp.CurrentRequestCosts += value;
            }
        }
        
        public DateTime BlockingTime { get; set; }
    }
}