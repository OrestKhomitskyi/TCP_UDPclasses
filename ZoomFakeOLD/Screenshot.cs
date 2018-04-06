using System;
using System.Drawing;
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
                bounds.Width = 300;
                bounds.Height = 300;
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
        public static BitmapSource ByteToBitMapSource(byte[] data)
        {
            var imageConverter = new ImageConverter();
            var image = (Image)imageConverter.ConvertFrom(data);

            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(((Bitmap)image).GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return bitmapSource;
        }

    }
}
