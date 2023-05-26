using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Saket.Serialization.Byte
{
    public class ByteReader
    {
        /// <summary> The number of bytes avaliable to the reader </summary>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (maxAbsolutePosition > 0)
                    return maxAbsolutePosition - offset;
                else
                    return data.Length - offset;
            }
        }

        /// <summary> The data that has been read by the reader </summary>
        public ArraySegment<byte> Data
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ArraySegment<byte>(data, offset, count);
        }
        public byte[] DataRaw
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => data;
        }

        public int RelativePosition
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => absolutePosition - offset;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => absolutePosition = value + offset;
        }
        /// <summary> The absolute position of reader in bytes based on underlying array</summary>
        public int AbsolutePosition
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => absolutePosition;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => absolutePosition = value;
        }

        public bool IsReader => throw new NotImplementedException();


        /// <summary> 
        /// Underlying array 
        /// </summary>
        byte[] data;
        /// <summary> 
        /// The current reader position relative to underlying array 
        /// </summary>
        int absolutePosition;
        /// <summary>
        /// The length of bytes read
        /// In effect this is the furthest (absolutePosition-offset) read from
        /// </summary>
        int count;
        /// <summary>
        /// The starting point for the reader. 
        /// The read should not be able to read bytes before this index
        /// </summary>
        readonly int offset;

        readonly int maxAbsolutePosition;

        public ByteReader(ArraySegment<byte> target)
        {
            data = target.Array;
            offset = absolutePosition = target.Offset;
            count = 0;
            maxAbsolutePosition = offset + target.Count;
        }

        public ByteReader(byte[] target, int offset = 0)
        {
            data = target;
            this.offset = absolutePosition = offset;
            count = 0;
            maxAbsolutePosition = -1;
        }





        // ---- Primitive Serialization ---- 
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Read<T>() where T : unmanaged
        {
            unsafe
            {
                fixed (byte* p = data)
                {
                    int position = absolutePosition;
                    Advance(Marshal.SizeOf<T>());
                    return *(T*)&p[position];
                }
            }
        }
        public T[] ReadArray<T>()
            where T : unmanaged
        {
            int length = Read<int>();
            if (length <= 0)
                return Array.Empty<T>();

            // Allocate new array
            // Todo make heap allocation free
            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Read<T>();
            }
            return result;
        }

        // ---- Serializable Serialization ----
        public T ReadSerializable<T>() where T : ISerializable, new()
        {

            throw new NotImplementedException();
        }
        public T[] ReadSerializableArray<T>() where T : ISerializable, new()
        {
            int length = Read<int>();

            if (length <= 0)
                return Array.Empty<T>();

            // Allocate new array
            // Todo make heap allocation free
            T[] result = new T[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = ReadSerializable<T>();
            }
            return result;
        }


        // ---- String Serialization ----
        public string ReadString()
        {
            int length = Read<int>();
            if (length == 0)
                return string.Empty;
            var segment = Read(length);
            return Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ArraySegment<byte> Read(int length)
        {
            var p = absolutePosition;
            Advance(length);
            return new ArraySegment<byte>(data, p, length);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64()
        {
            unsafe
            {
                fixed (byte* p = data)
                {
                    int position = absolutePosition;
                    Advance(8);
                    return *(ulong*)&p[position];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Advance(int length)
        {
            absolutePosition += length;
#if DEBUG
            if (maxAbsolutePosition > 0)
            {
                if (absolutePosition > maxAbsolutePosition)
                {
                    throw new IndexOutOfRangeException($"Read {absolutePosition - maxAbsolutePosition} bytes past underlying Buffer");
                }
            }
            else
            {
                if (absolutePosition > data.Length)
                {
                    throw new IndexOutOfRangeException($"Read {absolutePosition - data.Length} bytes past underlying Buffer");
                }
            }
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int SizeOf<T>()
        {
            Type outputType = typeof(T).IsEnum ? Enum.GetUnderlyingType(typeof(T)) : typeof(T);
            return Marshal.SizeOf(outputType);
        }

        public void SerializeUnmanaged<T>(ref T value) where T : unmanaged
        {
            throw new NotImplementedException();

        }

        public void SerializeEnum<T>(ref T value) where T : unmanaged, Enum
        {
            int position = absolutePosition;
            Type t = Enum.GetUnderlyingType(typeof(T));
            Advance(Marshal.SizeOf(t));
            unsafe
            {
                fixed (byte* p = data)
                {
                    value = (T)Marshal.PtrToStructure(new IntPtr(p + position), t)!;
                }
            }
        }

        public void SerializeSerializable<T>(ref T value) where T : ISerializable, new()
        {
            value = new T();
            //value.Serialize(this);
        }

        public void Serialize<T>(ref string value)
        {
            throw new NotImplementedException();
        }
    }
}
