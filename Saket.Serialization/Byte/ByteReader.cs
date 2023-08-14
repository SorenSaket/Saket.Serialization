using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Saket.Serialization.Byte
{
    /// <summary>
    /// Read directly from a byte Array.
    /// </summary>
    public struct ByteReader : ISerializer, IReader
    {
        #region Properties
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

        /// <summary> The data that is avaliable to the reader </summary>
        public ArraySegment<byte> Data
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ArraySegment<byte>(data, offset, bytesRead);
        }
        /// <summary>
        /// Get the underlying array of data, ignoring offset and count
        /// </summary>
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

        #endregion

        #region Variables

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
        int bytesRead;


        /// <summary>
        /// The starting point for the reader. 
        /// The read should not be able to read bytes before this index
        /// </summary>
        readonly int offset;
        /// <summary>
        /// The maximum position the reader can read from the underlying array. The reader should not read further than this.
        /// </summary>
        readonly int maxAbsolutePosition;
        /// <summary>
        /// The number of bytes avaliable for the reader
        /// </summary>
        readonly int bytesAvaliable;


        #endregion

        
        public ByteReader(ArraySegment<byte> target)
        {
            data = target.Array;
            offset = absolutePosition = target.Offset;
            bytesRead = 0;
            maxAbsolutePosition = offset + target.Count;
        }
        public ByteReader(byte[] target, int offset = 0, int count = 0)
        {
            this.data = target;
            this.offset = this.absolutePosition = offset;
            this.bytesRead = 0;
            

            if (count > 0)
            {
                this.maxAbsolutePosition = offset + count;
                this.bytesAvaliable = count;
            }
            else
            {
                this.bytesAvaliable = target.Length - offset;
                this.maxAbsolutePosition = target.Length;
            }
        }



        // ---- Serializable Serialization ----
        public T ReadSerializable<T>() where T : ISerializable, new()
        {
            T value = new T();
            value.Serialize(this);
            return value;
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
        public T[] ReadSerializableArray<T>(T[] arr) where T : ISerializable, new()
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



        #region IReader
        int IReader.Position { get => RelativePosition; set => RelativePosition = value; }

        public ArraySegment<byte> ReadBytes(int length)
        {
            var p = absolutePosition;
            Advance(length);
            return new ArraySegment<byte>(data, p, length);
        }

        // ---- Primitive Unmanaged Serialization ---- 
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
        // ---- String Serialization ----
        public string ReadString()
        {
            int length = Read<int>();
            if (length == 0)
                return string.Empty;
            var segment = ReadBytes(length);
            return Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
        }

        #endregion

        #region ISerializer
        long ISerializer.Position { get => RelativePosition; set => RelativePosition = (int)value; }
        bool ISerializer.IsReader => true;

        bool ISerializer.LoadBytes(int count)
        {
            throw new NotImplementedException();
        }

        void ISerializer.SerializeBytes(ref byte[] value, int offset, int count)
        {
            throw new NotImplementedException();
        }

        unsafe void ISerializer.SerializeBytes(byte* value, int count)
        {
            throw new NotImplementedException();
        }

        void ISerializer.Serialize<T>(ref T value)
        {
            unsafe
            {
                fixed (byte* p = data)
                {
                    value = Unsafe.Read<T>(p + absolutePosition);
                }
            }
        }

        void ISerializer.SerializeEnum<T>(ref T value)
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

        void ISerializer.SerializeSerializable<T>(ref T value)
        {
            value.Serialize(this);
        }

        #endregion

    }
}
