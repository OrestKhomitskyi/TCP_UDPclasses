using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace UDPServer
{
    class Program
    {
        public IPEndPoint IpEndPoint { get; set; }



        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter local ip and port");
                //local ip
                string ip = Console.ReadLine();
                int port = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Wrong Input"));

                UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Parse(ip), port));

                Task.Run(() =>
                {
                    IPEndPoint ep = null;
                    while (true)
                    {
                        byte[] getData = client.Receive(ref ep);
                        Console.WriteLine(Encoding.UTF8.GetString(getData));
                    }
                });
                //local ip
                Console.WriteLine("Enter remote ip and port");
                ip = Console.ReadLine();
                port = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Wrong Input"));
                while (true)
                {

                    IPEndPoint remote = new IPEndPoint(IPAddress.Parse(ip), port);
                    //Message
                    string message = Console.ReadLine();
                    client.Send(Encoding.UTF8.GetBytes(message), message.Length, remote);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



        }
    }
}
