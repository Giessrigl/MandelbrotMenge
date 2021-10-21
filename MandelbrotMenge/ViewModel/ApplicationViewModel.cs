using MandelbrotCommon;
using MandelbrotCommon.Interfaces;
using MandelbrotMenge.Commands;
using MandelbrotMenge.Interfaces;
using MandelbrotMenge.ResponseMapper;
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
    public class ApplicationViewModel
    {
        private readonly BitmapPainter bmpPainter;

        public ImageVM Image
        {
            get;
            private set;
        }

        private IRequestHandler reqHandler;

        private IResponseMapper<byte[], uint[,]> mapper;

        public CoordinateSystemAreaVM AffectedArea
        {
            get;
            set;
        }

        public uint Iterations
        {
            get;
            set;
        }

        public ApplicationViewModel()
        {
            this.bmpPainter = new BitmapPainter();
            this.reqHandler = new HttpRequestHandler();
            this.AffectedArea = new CoordinateSystemAreaVM(-2.15, 1, -1.5, 1.5);
            this.Image = new ImageVM(1900, 900);
            this.mapper = new MandelbrotMapper(this.Image);
            this.Iterations = 100;
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
                        var response = await this.reqHandler.PostMandelbrotAsync("https://localhost:44329",
                                new MandelbrotRequest()
                                {
                                    Width = this.Image.Width,
                                    Height = this.Image.Height,
                                    Left = this.AffectedArea.Limit_XAxis_Left,
                                    Right = this.AffectedArea.Limit_XAxis_Right,
                                    Top = this.AffectedArea.Limit_YAxis_Top,
                                    Bottom = this.AffectedArea.Limit_YAxis_Bottom,
                                    Iterations = this.Iterations
                                });

                        var map = this.mapper.Map(response);
                        this.Image.BmpImage = this.bmpPainter.PaintBitmap(map);
                    }
                );
            }
        }
    }
}
