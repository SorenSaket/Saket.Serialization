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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Saket.Serialization
{
   
    /// <summary>
    /// !Important call <see cref="SStreamReader.LoadBytes(int)"/> before calling any Serialize functions!
    /// </summary>
    public class SStreamReader : ISerializer
    {
        public long Position { get; set; }
        public Stream stream;
        public byte[] buffer = new byte[128];
        public bool IsReader => true;

        public SStreamReader(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            stream = input;
        }

        /// <summary>
        /// Ensures that there is more than the number of bytes left in stream.
        /// Do this once before any read to ensure that you don't cross end of stream.
        /// </summary>
        public virtual bool LoadBytes(int numberOfBytes)
        {
            if (stream == null)
                return false;
            if(numberOfBytes <= 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfBytes));


            int newCapacity = buffer.Length;
            // Double the capacity until theres enough
            while (numberOfBytes >= newCapacity)
            {
                newCapacity *= 2;
            }

            if(newCapacity > buffer.Length)
            {
                // Array.Resize is not used since the values should not be copied
                // values can stay uninitialized since they will be overwritten by stream.Read()
                // old array will be collected by GC
                buffer = GC.AllocateUninitializedArray<byte>(newCapacity);
            }

            // Reset poition
            Position = 0;
            int bytesRead = 0;
            int n = 0;
            do
            {
				
                n = stream.Read(buffer, bytesRead, numberOfBytes - bytesRead);
                if (n == 0)
                {
                    return false;
                }
                bytesRead += n;
            } while (bytesRead < numberOfBytes);

            return true;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void Advance(int length)
        {
            Position += length;
#if DEBUG
            if(Position > buffer.Length)
            {
                throw new IndexOutOfRangeException($"Read {Position - buffer.Length} bytes past underlying buffer.");
            }
#endif
        }

        /// <summary>
        /// offset and count are unused
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeBytes(ref byte[] value, int offset = 0, int count = 0)
        {

            int length = 0;
            LoadBytes(4);
            SerializeInt32(ref length);
            
            LoadBytes(length);


            if (value == null)
            {
                value = new byte[length];
                Array.Copy(buffer, Position, value, 0, length);
            }
            else
            {
                // resize array if it's smaller than requied
                int requiedSize = length;
                if (value.Length < requiedSize)
                    Array.Resize(ref value, requiedSize);

                Array.Copy(buffer, Position, value, 0, length);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SerializeBytes(ref byte* value, int count)
        {
            throw new NotImplementedException();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeBoolean(ref bool value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = p[Position] == 1;
                    Advance(1);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeByte(ref byte value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = p[Position];
                    Advance(1);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeSByte(ref sbyte value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = (sbyte)p[Position];
                    Advance(1);
                }
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeChar(ref char value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(char*)&p[Position];
                    Advance(2);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeString(ref string value)
        {
            LoadBytes(4);
            int length = 0;
            SerializeInt32(ref length);
            
            LoadBytes(length * 2);
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                SerializeChar(ref chars[i]);
            }

            value = new string(chars);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUInt16(ref ushort value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(ushort*)&p[Position];
                    Advance(2);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUInt32(ref uint value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(uint*)&p[Position];
                    Advance(4);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUInt64(ref ulong value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(ulong*)&p[Position];
                    Advance(8);
                }
            }
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeInt16(ref short value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(short*)&p[Position];
                    Advance(2);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeInt32(ref int value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(int*)&p[Position];
                    Advance(4);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeInt64(ref long value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(long*)&p[Position];
                    Advance(8);
                }
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeSingle(ref float value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(float*)&p[Position];
                    Advance(4);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeDouble(ref double value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(double*)&p[Position];
                    Advance(8);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeDecimal(ref decimal value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(decimal*)&p[Position];
                    Advance(16);
                }
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeVector2(ref Vector2 value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(Vector2*)&p[Position];
                    Advance(8);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeVector3(ref Vector3 value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(Vector3*)&p[Position];
                    Advance(12);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeVector4(ref Vector4 value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(Vector4*)&p[Position];
                    Advance(16);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeMatrix4x4(ref Matrix4x4 value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(Matrix4x4*)&p[Position];
                    Advance(64);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeQuaternion(ref Quaternion value)
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(Quaternion*)&p[Position];
                    Advance(16);
                }
            }
        }

        #region Generic Serialization Functions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUnmanaged<T>(ref T value) where T : unmanaged
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    value = *(T*)&p[Position];
                    Advance(Marshal.SizeOf<T>());
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeEnum<T>(ref T value) where T : unmanaged, Enum
        {
            unsafe
            {
                fixed (byte* p = buffer)
                {
                    Type t = Enum.GetUnderlyingType(typeof(T));
                    value = *(T*)&p[Position]; // Will this work?
                    Advance(Marshal.SizeOf(t));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeSerializable<T>(ref T value) where T : ISerializable, new()
        {
            value.Serialize(this);
        }


        #endregion
    }
}
