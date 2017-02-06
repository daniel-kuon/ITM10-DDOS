using System;
using System.Net;
using System.Net.Sockets;

namespace DDoSDetector
{
    public class IpRange
    {
        public IPAddress Start { get; }
        public IPAddress End { get; }

        public AddressFamily AddressFamily => Start.AddressFamily;

        public IpRange(IPAddress start, IPAddress end)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if (end == null) throw new ArgumentNullException(nameof(end));
            if (end.AddressFamily!=start.AddressFamily) throw new Exception("IpAddresses must be of the same version");
            Start = start;
            End = end;
        }

        public IpRange(IPAddress singleIpAddress)
        {
            Start = singleIpAddress;
            End = singleIpAddress;
        }

        public bool Contains(IPAddress address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));
            if (address.AddressFamily!=Start.AddressFamily) throw new Exception("IpAddresses must be of the same version");
            if (address.AddressFamily == AddressFamily.InterNetwork)
            {
#pragma warning disable 618
                return address.Address >= Start.Address && address.Address <= End.Address;
#pragma warning restore 618
            }
            if (ReferenceEquals(Start, End))
                return Start.Equals(address);
            byte[] startBytes = Start.GetAddressBytes();
            byte[] addressBytes = address.GetAddressBytes();
            for (var i = 0; i < addressBytes.Length; i++)
            {
                if (startBytes[i] > addressBytes[i])
                    return false;
            }
            byte[] endBytes = End.GetAddressBytes();
            for (var i = 0; i < addressBytes.Length; i++)
            {
                if (endBytes[i] < addressBytes[i])
                    return false;
            }
            return true;
        }
    }
}