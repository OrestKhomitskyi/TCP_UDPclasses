using System.Net;
using ZoomFake_TCP_.Media.Audio;
using ZoomFake_TCP_.Media.Audio.Codec;

namespace ZoomFake_TCP_.Media
{
    //Multicast
    public class AudioChat
    {
        private NetworkAudioPlayer NetworkAudioPlayer;
        private NetworkAudioSender NetworkAudioSender;
        private IAudioReceiver AudioReceiver;
        private IAudioSender AudioSender;
        private IPAddress IpAddress;
        private bool IsWorking = false;

        public AudioChat(IPAddress IpAddress)
        {
            this.IpAddress = IpAddress;
        }

        private void Receive()
        {
            AudioReceiver = new UdpAudioReceiver(IpAddress);
            NetworkAudioPlayer = new NetworkAudioPlayer(new NarrowBandSpeexCodec(), AudioReceiver);
        }

        private void Send()
        {
            AudioSender = new UdpAudioSender(IpAddress);
            NetworkAudioSender = new NetworkAudioSender(new NarrowBandSpeexCodec(), 0, AudioSender);
        }

        public void Start()
        {
            if (!IsWorking)
            {
                Receive();
                Send();
            }
            IsWorking = true;
        }

        public void Stop()
        {
            IsWorking = false;
            NetworkAudioSender?.Dispose();
            NetworkAudioPlayer?.Dispose();
        }

    }
}
