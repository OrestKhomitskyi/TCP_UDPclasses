using System;

namespace ZoomFake_TCP_
{
    [Serializable]
    public class FramePieceInfo
    {
        public byte[] FrameBytes { get; set; }
        public int TotalLength { get; set; }
        public readonly Guid Id;

        public FramePieceInfo(byte[] frameBytes, Guid id)
        {
            FrameBytes = frameBytes;
            Id = id;
        }

        ~FramePieceInfo()
        {
            FrameBytes = null;
        }
    }
}
