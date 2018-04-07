using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ZoomFake_TCP_.Media.Audio;
using ZoomFake_TCP_.Properties;

namespace ZoomFake_TCP_.Media
{
    class UdpAudioReceiver : IAudioReceiver
    {
        private Action<byte[]> handler;
        private readonly UdpClient udpListener;
        private bool listening;

        public UdpAudioReceiver(IPAddress IpAddress)
        {

            udpListener = new UdpClient(new IPEndPoint(IPAddress.Any, Settings.Default.portGroupAudio));
            udpListener.JoinMulticastGroup(IPAddress.Parse(Settings.Default.ipGroup));
            udpListener.MulticastLoopback = false;

            // To allow us to talk to ourselves for test purposes:
            // http://stackoverflow.com/questions/687868/sending-and-receiving-udp-packets-between-two-programs-on-the-same-computer
            //udpListener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            ThreadPool.QueueUserWorkItem(ListenerThread, null);
            listening = true;
        }

        private void ListenerThread(object obj)
        {
            IPEndPoint endPoint = (IPEndPoint)obj;
            try
            {
                while (listening)
                {
                    byte[] b = udpListener.Receive(ref endPoint);
                    handler?.Invoke(b);
                }
            }
            catch (SocketException)
            {
                // usually not a problem - just means we have disconnected
            }
        }

        public void Dispose()
        {
            listening = false;
            udpListener?.Close();
        }

        public void OnReceived(Action<byte[]> onAudioReceivedAction)
        {
            handler = onAudioReceivedAction;
        }
    }
}
