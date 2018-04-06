using System;
using System.Diagnostics;

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
            Debug.WriteLine("GC");
            FrameBytes = null;
        }
    }
}
