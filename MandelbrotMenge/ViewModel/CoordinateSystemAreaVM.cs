using MandelbrotMenge.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace MandelbrotMenge.ViewModel
{
    public class CoordinateSystemAreaVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double limit_XAxis_Left;

        private double limit_XAxis_Right;

        private double limit_YAxis_Bottom;

        private double limit_YAxis_Top;

        public double Limit_XAxis_Left
        {
            get
            {
                return this.limit_XAxis_Left;
            }
            set
            {
                this.limit_XAxis_Left = value;
                this.OnPropertyChanged();
            }
        }

        public double Limit_XAxis_Right
        {
            get
            {
                return this.limit_XAxis_Right;
            }
            set
            {
                this.limit_XAxis_Right = value;
                this.OnPropertyChanged();
            }
        }

        public double Limit_YAxis_Bottom
        {
            get
            {
                return this.limit_YAxis_Bottom;
            }
            set
            {
                this.limit_YAxis_Bottom = value;
                this.OnPropertyChanged();
            }
        }

        public double Limit_YAxis_Top
        {
            get
            {
                return this.limit_YAxis_Top;
            }
            set
            {
                this.limit_YAxis_Top = value;
                this.OnPropertyChanged();
            }
        }

        public CoordinateSystemAreaVM(double x1, double x2, double y1, double y2)
        {
            this.Limit_XAxis_Left = x1;
            this.limit_XAxis_Right = x2;
            this.Limit_YAxis_Bottom = y1;
            this.Limit_YAxis_Top = y2;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
