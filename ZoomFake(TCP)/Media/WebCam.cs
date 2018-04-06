using Emgu.CV;
using System;
using System.Diagnostics;
using System.Drawing;

namespace ZoomFake_TCP_
{
    public class WebCam : IDisposable
    {
        private VideoCapture vc = new VideoCapture();
        public byte[] GetVideoCaptureBytes()
        {

            try
            {
                Mat mat = new Mat();
                vc.Retrieve(mat);
                ImageConverter converter = new ImageConverter();
                Bitmap bp = mat.Bitmap;

                byte[] data = (byte[])converter.ConvertTo(bp, typeof(byte[]));
                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ERRRRRR");
            }
            return null;
        }
        public void Dispose()
        {
            vc.Dispose();
        }
    }
}
