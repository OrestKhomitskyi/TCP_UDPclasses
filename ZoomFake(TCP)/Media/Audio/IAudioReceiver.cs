using System;

namespace ZoomFake_TCP_.Media.Audio
{
    interface IAudioReceiver : IDisposable
    {
        void OnReceived(Action<byte[]> handler);
    }
}
