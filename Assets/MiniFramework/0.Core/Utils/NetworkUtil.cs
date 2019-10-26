using System.Net;
namespace MiniFramework
{
    public static class NetworkUtil
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