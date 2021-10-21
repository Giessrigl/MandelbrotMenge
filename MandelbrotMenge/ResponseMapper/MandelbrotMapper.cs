using MandelbrotCommon.Interfaces;
using MandelbrotMenge.ViewModel;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace MandelbrotMenge.ResponseMapper
{
    public class MandelbrotMapper : IResponseMapper<byte[], uint[,]>
    {
        private ImageVM image;

        public MandelbrotMapper(ImageVM image)
        {
            this.image = image;
        }

        public uint[,] Map(byte[] input)
        {
            uint[,] data = new uint[this.image.Width, this.image.Height];
            for (int y = 0; y < this.image.Height; y++)
            {
                for (int x = 0; x < this.image.Width; x++)
                {
                    int index = (y * this.image.Width + x) * 4;
                    data[x, y] = BinaryPrimitives.ReadUInt32LittleEndian(input.AsSpan().Slice(index, 4));
                }
            }

            return data;
        }
    }
}
