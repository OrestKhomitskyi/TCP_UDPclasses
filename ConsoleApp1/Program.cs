using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.IO;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TcpListener server = new TcpListener(IPAddress.Parse("192.168.190.70"), 1234);
                server.Start();

                TcpClient client= server.AcceptTcpClient();
                Console.WriteLine(client.Client.RemoteEndPoint.ToString());



                using (NetworkStream nsCl = client.GetStream())
                using(StreamReader sr=new StreamReader(nsCl,Encoding.UTF8))
                {
                    string success = sr.ReadToEnd();
                    Console.WriteLine(success);
                }
                client.Close();
                server.Stop();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                
            }
        }
    }
}
