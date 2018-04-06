using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Socket> socketsWhoList = new List<Socket>
            {
                new Socket(AddressFamily.AppleTalk, SocketType.Dgram, ProtocolType.Ggp),
                new Socket(AddressFamily.AppleTalk, SocketType.Dgram, ProtocolType.Ggp),
                new Socket(AddressFamily.AppleTalk, SocketType.Dgram, ProtocolType.Ggp),
                new Socket(AddressFamily.AppleTalk, SocketType.Dgram, ProtocolType.Ggp)
            };
            socketsWhoList[0].Blocking = true;





            try
            {
                TcpClient client = new TcpClient(new IPEndPoint(IPAddress.Parse("192.168.190.70"), 1123));
                client.Connect("192.168.190.70", 1234);

                using (NetworkStream ns = client.GetStream())
                using (StreamWriter sw = new StreamWriter(ns))
                {
                    sw.WriteLine(Console.ReadLine());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
