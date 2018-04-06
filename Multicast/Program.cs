using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Multicast
{
    class Program
    {
        private const string GROUP_ID = "224.5.6.7";

        static void Main()
        {
            Console.WriteLine("Receive");
            try
            {
                //Create UDP Socket
                Socket socket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Udp
                );

                IPAddress myIpAddress = IPAddress.Parse("192.168.190.70");

                //Bind address
                IPEndPoint endPoint = new IPEndPoint(myIpAddress, 2222);
                socket.Bind(endPoint);

                //Add to group
                IPAddress ipAddressGroup = IPAddress.Parse(GROUP_ID);
                socket.SetSocketOption(
                    SocketOptionLevel.IP,
                    SocketOptionName.AddMembership,
                    new MulticastOption(ipAddressGroup, myIpAddress)
                );
                //Sending data
                byte[] ar = new byte[1024];
                
                while (true)
                {
                    Console.WriteLine("Waiting Data");


                    int size = socket.Receive(ar);
                    var tmp = Encoding.UTF8.GetString(ar, 0, size);
                    Console.WriteLine($"Got a message: {tmp}");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
