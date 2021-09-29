using System;
using System.Collections.Generic;
using System.Text;

namespace MandelbrotMenge.MbC
{
    public class ComplexNumber
    {
        public double Real;
        public double Imaginary;

        public ComplexNumber(double real, double imaginary)
        {
            this.Real = real;
            this.Imaginary = imaginary;
        }

        public void Square()
        {
            double temp = (Real * Real) - (Imaginary * Imaginary);
            Imaginary = 2.0 * Real * Imaginary;
            Real = temp;
        }

        public double Magnitude()
        {
            return Math.Sqrt((Real * Real) + (Imaginary * Imaginary));
        }

        public void Add(ComplexNumber complex)
        {
            Real += complex.Real;
            Imaginary += complex.Imaginary;
        }
    }
}
