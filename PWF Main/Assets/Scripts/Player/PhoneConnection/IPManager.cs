using System.Net;

namespace Player.PhoneConnection
{
    public static class IPManager
    {
        public static IPAddress GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }

            throw new System.Exception("No network adapters with an IPv4 address in the system!");
        }

        public static IPEndPoint GetLocalEndpoint(int port = 80)
        {
            return new IPEndPoint(GetLocalIPAddress(), port);
        }
    }
}