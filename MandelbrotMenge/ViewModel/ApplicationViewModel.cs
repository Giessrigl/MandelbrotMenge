using MandelbrotCommon;
using MandelbrotCommon.Interfaces;
using MandelbrotMenge.Commands;
using MandelbrotMenge.Interfaces;
using MandelbrotMenge.ResponseMapper;
using MandelbrotMenge.Threading;
using System;
using System.Buffers.Binary;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Controls = System.Windows.Controls;

namespace MandelbrotMenge.ViewModel
{
    public class ApplicationViewModel
    {
        public ImageVM Image
        {
            get;
            private set;
        }

        private IRequestHandler reqHandler;

        private IResponseMapper<byte[], uint[,]> mapper;

        private Test test;

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
            
            this.reqHandler = new HttpRequestHandler();
            this.AffectedArea = new CoordinateSystemAreaVM(-2.15, 1, -1.5, 1.5);
            this.Image = new ImageVM(1900, 900);
            this.mapper = new MandelbrotMapper(this.Image);
            this.Iterations = 100;
            this.test = new Test(this.reqHandler, this.mapper);
        }

        public ICommand CalculateCommand
        {
            get
            {
                return new RelayCommand
                (
                    () => true,
                    () =>
                    {
                        this.test.TestMethod(this.Image, this.AffectedArea, this.Iterations);
                    }
                );
            }
        }
    }
}
