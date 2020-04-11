using System.Net;
using UnityEngine;

namespace MiniFramework
{
    public static class SocketUtil
    {
        public static IPAddress ParseIP(string address)
        {
            IPAddress ip;
            if (!IPAddress.TryParse(address, out ip))
            {
                IPHostEntry hostInfo = Dns.GetHostEntry(address);
                ip = hostInfo.AddressList[0];
            }
            return ip;
        }
    }
}