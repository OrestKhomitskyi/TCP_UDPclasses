using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ZoomFake
{
    public static class Screenshot
    {

        public static byte[] BitMapImageScreen
        {
            get
            {
                System.Drawing.Rectangle bounds = Screen.GetBounds(System.Drawing.Point.Empty);
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {

                        g.CopyFromScreen(System.Drawing.Point.Empty, System.Drawing.Point.Empty, bounds.Size);
                    }

                    ImageConverter converter = new ImageConverter();

                    byte[] data = (byte[])converter.ConvertTo(bitmap, typeof(byte[]));

                    return data;
                }
            }
        }

        private static void Vc_ImageGrabbed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource ByteToBitMapSource(byte[] data)
        {
            var imageConverter = new ImageConverter();
            Bitmap bitmap = (Bitmap)imageConverter.ConvertFrom(data);
            var hbitmap = bitmap.GetHbitmap();

            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hbitmap);
            }
            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    try
            //    {
            //        // You need to specify the image format to fill the stream. 
            //        // I'm assuming it is PNG
            //        bitmap.Save(memoryStream, ImageFormat.Png);
            //        memoryStream.Seek(0, SeekOrigin.Begin);

            //        BitmapDecoder bitmapDecoder = BitmapDecoder.Create(
            //            memoryStream,
            //            BitmapCreateOptions.PreservePixelFormat,
            //            BitmapCacheOption.OnLoad);

            //        // This will disconnect the stream from the image completely...
            //        WriteableBitmap writable =
            //            new WriteableBitmap(bitmapDecoder.Frames.Single());
            //        writable.Freeze();

            //        return writable;
            //    }
            //    catch (Exception)
            //    {
            //        return null;
            //    }
            //}
        }
    }
}
