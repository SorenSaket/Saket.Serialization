using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Saket.Serialization
{

    /// <summary>
    /// A stream reader that support preloading data into a buffer before interpreting it.
    /// !Important call <see cref="StreamReaderLE.LoadBytes(int)"/> before calling any Serialize functions!
    /// </summary>
    public class StreamReaderLE : BaseStreamReader, ISerializer, IReader
    {
        int IReader.Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public StreamReaderLE(Stream input) : base(input)
        {
        }

        #region Serialization
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
                Array.Copy(Buffer, Position, value, 0, length);
            }
            else
            {
                // resize array if it's smaller than requied
                int requiedSize = length;
                if (value.Length < requiedSize)
                    Array.Resize(ref value, requiedSize);

                Array.Copy(Buffer, Position, value, 0, length);
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
                {
                    value = *(Quaternion*)&p[Position];
                    Advance(16);
                }
            }
        }
        #endregion

        #region Generic Serialization Functions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void Serialize<T>(ref T value) where T : unmanaged
        {

#if DEBUG
            if(typeof(T).IsAssignableFrom(typeof(ISerializable)) ||
                typeof(T).IsAssignableFrom(typeof(ISerde)))
            {
                
                Console.WriteLine(
"You're serializing a type by blitting but the type implements A serializable interface. Make sure this is what you want to do");
            }

#endif
            unsafe
            {
                fixed (byte* p = Buffer)
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
                fixed (byte* p = Buffer)
                {
                    Type t = Enum.GetUnderlyingType(typeof(T));
                    value = *(T*)&p[Position]; // Will this work?
                    Advance(Marshal.SizeOf(t));
                }
            }
        }

        public unsafe void SerializeBytes(byte* value, int count)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region IReader

        public Span<byte> ReadBytes(int length)
        {
            throw new NotImplementedException();
        }

        public T Read<T>() where T : unmanaged
        {
            throw new NotImplementedException();
        }

        public T[] ReadArray<T>() where T : unmanaged
        {
            throw new NotImplementedException();
        }

        public T[] ReadArray<T>(T[] arr) where T : unmanaged
        {
            throw new NotImplementedException();
        }

        public string ReadString()
        {
            throw new NotImplementedException();
        }

        public T ReadSerializable<T>() where T : ISerializable
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
