using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SCUMMRevLib.Utils.Images;

namespace SCUMMRevLib.Utils
{
    public class ImageByteArray
    {
        protected int stride;
        public int Width { get; private set; }
        public int Height { get; private set; }

        public byte[] Bitmap { get; private set; }

        public ImageByteArray(int width, int height, byte bytesPerPixel)
        {
            Width = width;
            stride = Width*bytesPerPixel;
            Height = height;
            Bitmap = new byte[width * height * bytesPerPixel];
        }
    }

    public class ImageByteArray32 : ImageByteArray
    {
        public ImageByteArray32(int width, int height) : base(width, height, 4)
        {
            
        }

        public void SetPixel(int x, int y, Color color)
        {
            Bitmap[y*stride + x] = color.A;
            Bitmap[y * stride + x + 1] = color.B;
            Bitmap[y * stride + x + 2] = color.G;
            Bitmap[y * stride + x + 3] = color.R;
        }
    }
}
