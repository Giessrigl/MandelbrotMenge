using MandelbrotMenge.MbC;
using MandelbrotMenge.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace MandelbrotMenge.ViewModel
{
    public class MandelbrotCalculator : IMandelbrotCalculator, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int steps_XAxis;

        private int steps_YAxis;

        public int Steps_XAxis 
        { 
            get
            {
                return steps_XAxis;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.steps_XAxis = value;
                this.OnPropertyChanged();
            }
        }

        public int Steps_YAxis
        {
            get
            {
                return steps_YAxis;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.steps_YAxis = value;
                this.OnPropertyChanged();
            }
        }

        public MandelbrotCalculator()
        {
            this.Steps_XAxis = 2000;
            this.steps_YAxis = 1000;
        }

        public int[,] Calculate(CoordinateSystemArea csArea)
        {
            int[,] MBMap = new int[steps_XAxis + 1, steps_YAxis + 1];

            double deltaX = csArea.limit_XAxis_Left - csArea.limit_XAxis_Right;
            deltaX = deltaX < 0 ? deltaX * (-1) : deltaX;
            double stepSizeX = deltaX / steps_XAxis;

            double deltaY = csArea.limit_YAxis_Bottom - csArea.limit_YAxis_Top;
            deltaY = deltaY < 0 ? deltaY * (-1) : deltaY;
            double stepSizeY = deltaY / steps_YAxis;


            int counter_stepsXAxis = 0;
            for (double i = csArea.limit_XAxis_Left; 
                i < csArea.limit_XAxis_Right; 
                i += stepSizeX, 
                counter_stepsXAxis++)
            {
                int counter_stepsYAxis = 0;
                for (double j = csArea.limit_YAxis_Bottom; 
                    j < csArea.limit_YAxis_Top; 
                    j += stepSizeY, 
                    counter_stepsYAxis++)
                {
                    ComplexNumber c = new ComplexNumber(i, j);
                    ComplexNumber z = new ComplexNumber(0, 0);

                    int iteration = 0;
                    do
                    {
                        iteration++;
                        z.Square();
                        z.Add(c);
                        if (z.Magnitude() > 2.0)
                            break;
                    }
                    while (iteration < 100);

                    MBMap[counter_stepsXAxis, counter_stepsYAxis] = iteration;
                }
            }

            return MBMap;
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
