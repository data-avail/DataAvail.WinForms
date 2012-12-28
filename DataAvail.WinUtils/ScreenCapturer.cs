using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace DataAvail.WinUtils
{
    public static class ScreenCapturer
    {
        public static System.Drawing.Bitmap Capture(Rectangle Rectangle, string FileSave, System.Drawing.Imaging.ImageFormat ImageFormat)
        {
            Rectangle bounds = Rectangle == Rectangle.Empty ? System.Windows.Forms.Screen.PrimaryScreen.Bounds : Rectangle;

            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap)) 
            {
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            }

            if (!string.IsNullOrEmpty(FileSave))
            {
                bitmap.Save(FileSave, ImageFormat);
            }

            return bitmap;
        }

        public static System.Drawing.Bitmap Capture(string FileSave, System.Drawing.Imaging.ImageFormat ImageFormat)
        { 
            return Capture(Rectangle.Empty, FileSave, ImageFormat);
        }

        public static System.Drawing.Bitmap Capture(System.Windows.Forms.Form Form, string FileSave, System.Drawing.Imaging.ImageFormat ImageFormat)
        {
            return Capture(Form != null ? Form.Bounds : Rectangle.Empty, FileSave, ImageFormat);
        }

    }
}
