using System.Net;
using System.Net.Sockets;
using ZoomFake_TCP_.Media.Audio;
using ZoomFake_TCP_.Properties;

namespace ZoomFake_TCP_.Media
{
    class UdpAudioSender : IAudioSender
    {
        private readonly UdpClient udpSender;
        private IPAddress IpAddress;


        public UdpAudioSender(IPAddress IpAddress)
        {
            udpSender = new UdpClient();
            udpSender.MulticastLoopback = false;
            //udpSender.JoinMulticastGroup(IPAddress.Parse(Settings.Default.ipGroup));
            udpSender.Connect(Settings.Default.ipGroup, Settings.Default.portGroupAudio);
        }

        public void Send(byte[] payload)
        {
            udpSender.Send(payload, payload.Length);
        }

        public void Dispose()
        {
            udpSender?.Close();
        }
    }
}
