using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using DDoSDetector.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace DDoSDetector
{
    public static class Repository
    {

        //private static readonly Dictionary<string, Request> _RunningRequests = new Dictionary<string, Request>();
        //private static readonly List<Request> _Requests = new List<Request>();
        //public static IReadOnlyCollection<Request> Requests => _Requests.AsReadOnly();
        private static readonly List<ClientIp> _ClientIps = new List<ClientIp>();
        public static IReadOnlyCollection<ClientIp> ClientIps => _ClientIps.AsReadOnly();
        public static Configuration Configuration { get; set; }

        //private static readonly List<Request> _NewRequests = new List<Request>();
        //private static readonly List<ClientIp> _NewClientIps = new List<ClientIp>();

        public static Request AddRequest(HttpRequest httpRequest)
        {
            var request = new Request(httpRequest);
            //_Requests.Add(request);
            //_NewRequests.Add(request);
            return request;
        }

        //public static Request SetRequestState(string httpRequestId, RequestState state)
        //{
        //    var request = GetRequest(httpRequestId);
        //    request.State = state;
        //    return request;
        //}

        //public static Request GetRequest(string httpRequestId)
        //{
        //    if (_RunningRequests.ContainsKey(httpRequestId))
        //        return _RunningRequests[httpRequestId];
        //    return _Requests.FirstOrDefault(r => r.HttpRequestId == httpRequestId);
        //}

        public static ClientIp GetClientIp(IPAddress address)
        {
            var clientIp = _ClientIps.FirstOrDefault(c => c.Ip.Equals(address));
            return clientIp ?? AddClientIp(address);
        }

        private static ClientIp AddClientIp(IPAddress address)
        {
            var clientIp = new ClientIp()
            {
                Ip = address,
                BinaryIp = GetBinaryIp(address),
                IsIpV6=address.GetAddressBytes().Length!=4,
                
            };
            clientIp.IsWhiteListed =
                Configuration.WhiteList.Any(
                    range => range.AddressFamily == clientIp.Ip.AddressFamily && range.Contains(clientIp.Ip));
            clientIp.IsBlackListed =
                Configuration.BlackList.Any(
                    range => range.AddressFamily == clientIp.Ip.AddressFamily && range.Contains(clientIp.Ip));
            if (clientIp.IsWhiteListed && clientIp.IsBlackListed)
                throw new Exception($"IP {address} cannot be both black and white listed");
            //_NewClientIps.Add(clientIp);
            _ClientIps.Add(clientIp);
            return clientIp;
        }

        private static string GetBinaryIp(IPAddress address)
        {
            var output = "";
            foreach (byte originalAddressByte in address.GetAddressBytes())
            {
                var byteOutput = "";
                var addressByte = originalAddressByte;
                while (addressByte > 0)
                {
                    byteOutput = addressByte % 2 + output;
                    addressByte /= 2;
                }
                output += byteOutput.PadLeft(8, '0');
            }
            return output;
        }

        public static void RemoveRequest(Request request)
        {
            request.ClientIp.Requests.Remove(request);
            request.ClientIp.CurrentRequestCosts -= request.Costs;
        }
    }
}