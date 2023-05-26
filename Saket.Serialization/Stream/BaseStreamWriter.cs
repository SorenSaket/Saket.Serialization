using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseStreamWriter
    {
        /// <summary>
        /// Position of the underlying stream
        /// </summary>
        public long Position { get=> Stream.Position; set => Stream.Position = value; }
        /// <summary>
        /// Underlying Stream
        /// </summary>
        public Stream Stream { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Buffer { get; protected set; }

        public bool IsReader => false;

        public BaseStreamWriter(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            Stream = input;
            Buffer = new byte[64];
        }

        /// <summary>
        /// Writer doesn't need to load bytes.
        /// </summary>
        /// <param name="numberOfBytes"></param>
        /// <returns></returns>
        public virtual bool LoadBytes(int numberOfBytes)
        {
            return true;
        }

    }
}