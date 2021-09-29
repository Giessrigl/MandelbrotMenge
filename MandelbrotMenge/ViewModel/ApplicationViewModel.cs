using MandelbrotMenge.MbC;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Controls = System.Windows.Controls;

namespace MandelbrotMenge.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private readonly BitmapPainter bmp;

        public IMandelbrotCalculator MBC
        {
            get;
            private set;
        }

        public CoordinateSystemAreaVM AffectedArea
        {
            get;
            set;
        }

        private BitmapImage bmpImage;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public ApplicationViewModel()
        {
            this.MBC = new MandelbrotCalculator();
            this.bmp = new BitmapPainter();
            this.AffectedArea = new CoordinateSystemAreaVM(-2.15, 1, -1.5, 1.5);
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
                        var map = MBC.Calculate(new CoordinateSystemArea(
                            AffectedArea.Limit_XAxis_Left,
                            AffectedArea.Limit_XAxis_Right, 
                            AffectedArea.Limit_YAxis_Bottom, 
                            AffectedArea.Limit_YAxis_Top));

                        this.BmpImage = this.bmp.PaintBitmap(map);
                    }
                );
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
