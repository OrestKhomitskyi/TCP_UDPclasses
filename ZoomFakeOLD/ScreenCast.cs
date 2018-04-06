using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ZoomFake
{
    class ScreenCast : Connection
    {
        public event Action<byte[]> FrameChange;
        public string Ip { get; set; }
        private int port = 3333;
        private UdpClient client;


        public ScreenCast(string Ip)
        {
            this.Ip = Ip;
            client = new UdpClient();
            client.Client.Bind(new IPEndPoint(IPAddress.Parse(Ip), port));
            client.JoinMulticastGroup(GrouIpAddress);

        }

        public override void Send(byte[] ScreenBytes)
        {

        }

        public override async void Receive()
        {
            Task task = new Task(Pending);
            task.Start();
            await task;
        }

        public async void StartCasting()
        {
            Task task = new Task(Casting);
            task.Start();
            await task;
        }

        private void Casting()
        {
            //Іп групи
            IPEndPoint groupEndPoint = new IPEndPoint(GrouIpAddress, port);
            while (true)
            {
                try
                {
                    //Скріншот
                    byte[] data = Screenshot.BitMapImageScreen;
                    client.Send(data, data.Length, groupEndPoint);
                    Thread.Sleep(10);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void Pending()
        {

            byte[] frame;
            IPEndPoint remote = null;
            while (true)
            {
                try
                {
                    //Отримання першого флажка SIB
                    frame = client.Receive(ref remote);
                    FrameChange(frame);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
