using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ZoomFake_TCP_
{
    class GroupChat : GroupMulticast
    {
        private readonly UdpClient ClientReceive;
        private readonly UdpClient ClientSend;

        public event Action<Message> OnMessage;
        private CancellationTokenSource cancellationTokenSource;


        public GroupChat(IPAddress IpAddress)
        {
            ClientReceive = new UdpClient(new IPEndPoint(IPAddress.Any, PortChat));
            ClientReceive.JoinMulticastGroup(GroupIp);

            ClientSend = new UdpClient();
            ClientSend.JoinMulticastGroup(GroupIp);
            ClientSend.Connect(new IPEndPoint(GroupIp, PortChat));

            cancellationTokenSource = new CancellationTokenSource();
        }

        public void StopChat()
        {
            cancellationTokenSource.Cancel();
            ClientSend.Close();
            ClientReceive.Close();
        }

        private async void Receive()
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    UdpReceiveResult result = await ClientReceive.ReceiveAsync();
                    byte[] data = result.Buffer;

                    if (data.Length > 0)
                    {
                        Message message = new Message()
                        {
                            Address = result.RemoteEndPoint.Address,
                            Data = Encoding.UTF8.GetString(data)
                        };
                        OnMessage(message);
                    }
                }
                catch (ObjectDisposedException ode)
                {
                    Debug.WriteLine("Group Chat Socket closed");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public async void ReceiveAsync()
        {
            await Task.Factory.StartNew(Receive, cancellationTokenSource.Token);
        }

        public void Send(byte[] Data)
        {
            try
            {
                ClientSend.Send(Data, Data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
