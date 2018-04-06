using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MulticastSender
{
    class Program
    {
        private const string GROUP_ID = "224.5.6.7";

        static void Main(string[] args)
        {
            Socket socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Dgram,
                ProtocolType.Udp
            );

            IPAddress myIpAddress = IPAddress.Parse("192.168.190.70");
            IPAddress ipAddressGroup = IPAddress.Parse(GROUP_ID);

            socket.SetSocketOption(
                SocketOptionLevel.IP,
                SocketOptionName.AddMembership,
                new MulticastOption(ipAddressGroup)
                );

            IPEndPoint epep = new IPEndPoint(ipAddressGroup, 2222);
            socket.Connect(epep);

            socket.Connect(epep);

            while (true)
            {
                Console.WriteLine("Enter: ");
                string data = Console.ReadLine();
                byte[] d = Encoding.UTF8.GetBytes(data);

                socket.Send(d, d.Length, SocketFlags.None);

            }

        }
    }
}
