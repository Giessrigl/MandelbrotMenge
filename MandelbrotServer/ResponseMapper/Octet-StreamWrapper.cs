using MandelbrotCommon.Interfaces;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MandelbrotServer.ResponseWrapper
{
    public class OctetStreamMapper : IResponseMapper<uint[], byte[]>
    {
        public byte[] Map(uint[] input)
        {
            var length = input.Length;
            Span<byte> result = new Span<byte>(new byte[4]);
            List<byte> output = new List<byte>(length * 4);

            for (int i = 0; i < length; i++)
            {
                if (!BinaryPrimitives.TryWriteUInt32LittleEndian(result, input[i]))
                {
                    throw new ArgumentOutOfRangeException(nameof(result), "Could not write all bytes into the byte array.");
                }

                output.AddRange(result.ToArray());
            }

            return output.ToArray();
        }
    }
}
