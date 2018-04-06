using System;
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
            Client.JoinMulticastGroup(GroupIp);
            cancellationToken = new CancellationTokenSource();
        }

        public void StopChat()
        {
            cancellationToken.Cancel();
            Client.Close();
        }

        private void Receive()
        {
            IPEndPoint remote = null;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    byte[] data = Client.Receive(ref remote);

                    if (data.Length > 0)
                    {
                        Message message = new Message()
                        {
                            Address = remote.Address,
                            Data = Encoding.UTF8.GetString(data)
                        };
                        OnMessage(message);
                    }
                }
                catch (SocketException ex)
                {

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
