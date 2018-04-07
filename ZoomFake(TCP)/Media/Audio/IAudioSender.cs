using System;

namespace ZoomFake_TCP_.Media.Audio
{
    interface IAudioSender : IDisposable
    {
        void Send(byte[] payload);
    }
}
