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

        public uint[,] Map(byte[] input, int width, int height)
        {
            uint[,] data = new uint[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = (y * width + x) * 4;
                    data[x, y] = BinaryPrimitives.ReadUInt32LittleEndian(input.AsSpan().Slice(index, 4));
                }
            }

            return data;
        }
    }
}
