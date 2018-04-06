using System;
using System.IO;

namespace ZoomFake_TCP_
{
    [Serializable]
    public class FilePiece
    {
        public byte[] Data { get; set; }
        public readonly FileInfo FileInfo;

        public FilePiece(FileInfo fi, byte[] Data)
        {
            this.Data = Data;
            this.FileInfo = fi;
        }
    }
}
