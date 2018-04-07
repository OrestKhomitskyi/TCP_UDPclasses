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
        private readonly UdpClient Client;
        public event Action<Message> OnMessage;
        private CancellationTokenSource cancellationToken;


        public GroupChat(IPAddress IpAddress)
        {
            Client = new UdpClient(new IPEndPoint(IpAddress, PortChat));
            ////Client.ExclusiveAddressUse = false;
            //Client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            //Client.ExclusiveAddressUse = false;
            //Client.Client.Bind(new IPEndPoint(IpAddress, PortChat));

            Client.JoinMulticastGroup(GroupIp);
            cancellationToken = new CancellationTokenSource();
        }

        public void StopChat()
        {
            cancellationToken.Cancel();
            Client.Close();
        }

        private async void Receive()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    UdpReceiveResult result = await Client.ReceiveAsync();
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
            await Task.Factory.StartNew(Receive, cancellationToken.Token);
        }

        public void Send(byte[] Data)
        {
            try
            {
                Client.Send(Data, Data.Length, new IPEndPoint(GroupIp, PortChat));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
