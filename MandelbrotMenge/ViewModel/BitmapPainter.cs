using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media.Imaging;

namespace MandelbrotMenge.ViewModel
{
    public class BitmapPainter
    {
        public BitmapImage PaintBitmap(uint[,] map)
        {
            Bitmap bmp = new Bitmap(map.GetLength(0), map.GetLength(1));

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    bmp.SetPixel(i, j, ChoosePaint(map[i, j]));
                }
            }

            return this.BitmapToImageSource(bmp);
        }

        private Color ChoosePaint(uint iterations) => iterations switch
        {
            var x when x < 1 => Color.White,
            var x when x < 2 => Color.AntiqueWhite,
            var x when x < 3 => Color.LightGray,
            var x when x < 4 => Color.DarkGray,
            var x when x < 5 => Color.Gray,

            var x when x < 6 => Color.OrangeRed,
            var x when x < 7 => Color.Red,
            var x when x < 8 => Color.DarkRed,

            var x when x < 9 => Color.LightBlue,
            var x when x < 10 => Color.Blue,
            var x when x < 30 => Color.DarkBlue,
            var x when x < 50 => Color.LightGreen,
            var x when x < 70 => Color.Green,
            var x when x < 100 => Color.DarkGreen,
            _ => Color.Black
        };

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
