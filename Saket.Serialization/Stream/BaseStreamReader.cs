using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Saket.Serialization
{
    public abstract class BaseStreamReader 
    {
        #region Variables

        /// <summary>
        /// Position within the internal buffer
        /// </summary>
        public long Position { get; set; }
        /// <summary>
        /// Underlying Stream
        /// </summary>
        public Stream Stream { get; protected set; }
        /// <summary>
        /// Internal buffer
        /// </summary>
        public byte[] Buffer { get; protected set; } = new byte[128];

        public bool IsReader => true;

        #endregion


        private int currentBufferContentLength;

        public BaseStreamReader(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            Stream = input;
        }

        /// <summary>
        /// Ensures that there is more than the number of bytes left in stream.
        /// Do this once before any read to ensure that you don't cross end of stream.
        /// </summary>
        public virtual bool LoadBytes(int numberOfBytes)
        {
            if (Stream == null)
                return false;
            if(numberOfBytes <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfBytes));


            int newCapacity = Buffer.Length;
            // Double the capacity until theres enough
            while (numberOfBytes >= newCapacity)
            {
                newCapacity *= 2;
            }

            if(newCapacity > Buffer.Length)
            {
                // Array.Resize is not used since the values should not be copied
                // values can stay uninitialized since they will be overwritten by stream.Read()
                // old array will be collected by GC
                Buffer = GC.AllocateUninitializedArray<byte>(newCapacity);
            }
            Stream.ReadExactly(Buffer, 0, numberOfBytes);
            currentBufferContentLength = numberOfBytes;
            Position = 0;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void Advance(int length)
        {
            Position += length;

            if(Position > Buffer.Length ||Position > currentBufferContentLength)
            {
                throw new IndexOutOfRangeException($"Read {Position - Buffer.Length} bytes past underlying Buffer.");
            }
        }
     
    }
}
