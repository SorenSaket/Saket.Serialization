using System;
using System.Net;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Saket.Serialization.Byte
{
    /// <summary>
    /// Writer struct able to serialize primities and ISerializables to byte array
    /// </summary>
    public struct ByteWriter : ISerializer, IWriter
    {
        #region Properties
        /// <summary> The number of bytes avaliable to the writer </summary>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => data.Length;
        }
        /// <summary> The absolute position of writer in bytes based on underlying array</summary>
        public int AbsolutePosition
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => absolutePosition;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => absolutePosition = value;
        }
        /// <summary> How many bytes the writer has written. </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => count;
        }
        /// <summary> The data that has been written to the underlying array </summary>
        public ArraySegment<byte> Data
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new ArraySegment<byte>(data, 0, count);
        }
        /// <summary>  </summary>
        public byte[] DataRaw
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => data;
        }

        #endregion

        #region Variables
        /// <summary> 
        /// Underlying array 
        /// </summary>
        byte[] data;

        /// <summary> 
        /// The current writer position relative to underlying array 
        /// </summary>
        int absolutePosition;

        /// <summary>
        /// The length of bytes written
        /// In effect this is the furthest (absolutePosition-offset) written to
        /// </summary>
        int count;

        #endregion


        public ByteWriter(byte[] data)
        {
            this.data = data;
            count = 0;
            
        }
        public ByteWriter(int intialCapacity = 64)
        {
            data = new byte[intialCapacity];
            count = 0;
        }


        // ---- Primitive Serialization ----
        public unsafe void Write<T>(in T value)
            where T : unmanaged
        {
            fixed (T* ptr = &value)
            {
                Write(ptr, SizeOf<T>());
            }
        }
        public unsafe void Write<T>(in T[] value)
            where T : unmanaged
        {
            if (value == null || value.Length <= 0)
            {
                Write(0);
                return;
            }

            Write<int>(value.Length);
            int size = SizeOf<T>();
            fixed (T* ptr = value)
            {
                Write(ptr, size * value.Length);
            }
        }
        public unsafe void Write<T>(in ArraySegment<T> value)
            where T : unmanaged
        {
            Write(value.Count);
            int size = SizeOf<T>();
            fixed (T* ptr = value.Array)
            {
                Write(ptr + value.Offset * size, size * value.Count);
            }
        }

        // ---- Serializable Serialization ----
        public void WriteSerializable<T>(in T value) where T : ISerializable, new()
        {
            value.Serialize(this);
        }
        public void WriteSerializable<T>(in T[] value) where T : ISerializable, new()
        {
            Write(value.Length);
            for (int i = 0; i < value.Length; i++)
            {
                //value[i].Serialize(this);
            }
        }

        // ---- String Serialization ----
        public unsafe void Write(string value)
        {
            Write(value.Length);
            fixed (char* native = value)
            {
                Write(native, value.Length * sizeof(char));
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void Write(void* value, int length)
        {
            Marshal.Copy(new IntPtr(value), data, absolutePosition, length);
            absolutePosition += length;
            // 
            count = Math.Max(count, absolutePosition);
        }



        public void Reset()
        {
            absolutePosition = 0;
            count = 0;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int SizeOf<T>()
        {
            Type outputType = typeof(T).IsEnum ? Enum.GetUnderlyingType(typeof(T)) : typeof(T);
            return Marshal.SizeOf(outputType);
        }



        public bool LoadBytes(int count)
        {
            if (data.Length < absolutePosition + count)
                return false;
            return true;
        }


        #region IWriter
        public void WriteSerde<T>(in T value) where T : ISerde, new()
        {
            throw new NotImplementedException();
        }

        public void WriteSerde<T>(in T[] value) where T : ISerde, new()
        {
            throw new NotImplementedException();
        }


        int IWriter.Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public unsafe void SerializeBytes(byte* value, int count)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region ISerializer

        bool ISerializer.IsReader => false;
        long ISerializer.Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            throw new NotImplementedException();
        }

        void ISerializer.SerializeEnum<T>(ref T value)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}