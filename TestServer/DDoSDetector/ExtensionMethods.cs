using System;
using System.Net;
using System.Net.Sockets;

namespace DDoSDetector
{
    public static class ExtensionMethods
    {
        public static bool IsIpV4(this IPAddress ipAddress)
        {
            if (ipAddress == null) throw new ArgumentNullException(nameof(ipAddress));
            return ipAddress.AddressFamily == AddressFamily.InterNetwork;
        }
        public static bool IsIpV6(this IPAddress ipAddress)
        {
            if (ipAddress == null) throw new ArgumentNullException(nameof(ipAddress));
            return ipAddress.AddressFamily == AddressFamily.InterNetworkV6;
        }
    }
}