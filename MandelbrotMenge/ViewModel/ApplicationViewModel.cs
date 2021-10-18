using MandelbrotCommon;
using MandelbrotMenge.Interfaces;
using System;
using System.Buffers.Binary;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Controls = System.Windows.Controls;

namespace MandelbrotMenge.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly BitmapPainter bmpPainter;

        private BitmapImage bmpImage;

        private IRequestHandler reqHandler;

        public int Width
        {
            get;
            set;
        }

        public int Height
        {
            get;
            set;
        }

        public CoordinateSystemAreaVM AffectedArea
        {
            get;
            set;
        }

        public BitmapImage BmpImage
        {
            get
            {
                return this.bmpImage;
            }
            private set
            {
                if (value != null)
                {
                    this.bmpImage = value;
                    this.OnPropertyChanged();
                }
            }
        }

        public uint Iterations
        {
            get;
            set;
        }

        public ApplicationViewModel()
        {
            this.Width = 2000;
            this.Height = 1000;
            this.bmpPainter = new BitmapPainter();
            this.reqHandler = new HttpRequestHandler();
            this.AffectedArea = new CoordinateSystemAreaVM(-2.15, 1, -1.5, 1.5);
        }

        public ICommand CalculateCommand
        {
            get
            {
                return new RelayCommand
                (
                    () => true,
                    async () =>
                    {
                        var response = await this.reqHandler.PostAsync("https://localhost:44329",
                                new MandelbrotRequest()
                                {
                                    Width = this.Width,
                                    Height = this.Height,
                                    Left = this.AffectedArea.Limit_XAxis_Left,
                                    Right = this.AffectedArea.Limit_XAxis_Right,
                                    Top = this.AffectedArea.Limit_YAxis_Top,
                                    Bottom = this.AffectedArea.Limit_YAxis_Bottom,
                                    Iterations = this.Iterations
                                });

                        var map = this.Test(response, this.Width, this.Height);
                        this.BmpImage = this.bmpPainter.PaintBitmap(map);
                    }
                );
            }
        }

        private uint[,] Test(byte[] bytes, int width, int height)
        {
            uint[,] data = new uint[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = (y * width + x) * 4;
                    data[x,y] = BinaryPrimitives.ReadUInt32LittleEndian(bytes.AsSpan().Slice(index, 4));
                }
            }

            return data;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
