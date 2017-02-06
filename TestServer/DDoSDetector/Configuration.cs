using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using DDoSDetector.Multipliers;

namespace DDoSDetector
{
    public class Configuration
    {
        private static Configuration _default;
        public int CostKeepingTime { get; set; }
        public int DefaultRequestCosts { get; set; }
        public Dictionary<int, int> HttpErrorCosts { get; set; }
        public Dictionary<Regex, int> UrlPathCosts { get; set; }
        public int BlockingLimitPerIp { get; set; }
        public double BlockingLimitPerNetwork { get; set; }
        public int IpV4NetworkStepSize { get; set; }
        public int IpV6NetworkStepSize { get; set; }
        public int IpV4InitialNetworkSize { get; set; }
        public int IpV6InitialNetworkSize { get; set; }
        public List<IpRange> WhiteList { get; set; } = new List<IpRange>();
        public List<IpRange> BlackList { get; set; } = new List<IpRange>();
        public List<ICostMultiplier> CostMultipliers { get; set; } = new List<ICostMultiplier>();

        public static Configuration GetDefaultConfiguration()
        {
            return new Configuration
            {
                BlockingLimitPerIp = 2000,
                BlockingLimitPerNetwork = 30,
                CostKeepingTime = 10,
                DefaultRequestCosts = 10,
                HttpErrorCosts = new Dictionary<int, int>
                {
                    {500, 100},
                    {404, 30}
                },
                UrlPathCosts = new Dictionary<Regex, int>
                {
                    {new Regex(@"^\/(?:lib|css|js|img)\/"), 5}
                },
                IpV4NetworkStepSize = 1,
                IpV6NetworkStepSize = 1,
                IpV4InitialNetworkSize = 3,
                IpV6InitialNetworkSize = 3,
                WhiteList =
                    new List<IpRange>
                    {
                        new IpRange(new IPAddress(new byte[] {127, 0, 0, 1})),
                        new IpRange(IPAddress.IPv6Loopback)
                    },
                CostMultipliers = new List<ICostMultiplier>()
                //{
                //    new ResponseDurationCostMultiplier(10)
                //}
            };
        }
    }
}