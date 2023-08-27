using System;
using System.Numerics;


namespace Saket.Serialization
{
    /// <summary>
    /// A Reader can convert a series of bytes into primitive values.
    /// A Reader automatically advances position when reading values.
    /// </summary>
    public interface IReader
    {
        public int Position { get; set; }
        
        /// <summary>
        /// Read number of bytes 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public Span<byte> ReadBytes(int length);

        // Generic
        public T Read<T>() where T : unmanaged;
        public T[] ReadArray<T>() where T : unmanaged
        {
            int length = Read<int>();
            if (length <= 0)
                return Array.Empty<T>();

            T[] result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = Read<T>();
            }
            return result;
        }
        public T[] ReadArray<T>(T[] arr) where T : unmanaged 
        {
            int length = Read<int>();
           
            length = Math.Min(length, arr.Length);

            for (int i = 0; i < length; i++)
            {
                arr[i] = Read<T>();
            }
            return arr;
        }

        // Serde
        public T ReadSerde<T>() where T : ISerde, new()
        {
            var value = new T();
            value.Deserialize(this);
            return value;
        }
        public T[] ReadSerdeArray<T>() where T : ISerde, new()
        {
            int length = Read<int>();
            T[] array = new T[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = ReadSerde<T>();
            }
            return array;
        }
        /// <summary>
        /// Fills the given array with values. If the array size is less than the number of values data will be lost
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arr"></param>
        /// <returns></returns>
        public T[] ReadSerdeArray<T>(T[] arr) where T : ISerde, new()
        {
            int length = Read<int>();

            length = Math.Min(length, arr.Length);

            for (int i = 0; i < length; i++)
            {
                arr[i] = ReadSerde<T>();
            }
            return arr;
        }




        public string ReadString();


        /*
        public bool ReadBoolean();
        public byte ReadByte();
        public sbyte ReadSByte();
        public Char ReadChar();

        public UInt16 ReadUInt16();
        public UInt32 ReadUInt32();
        public UInt64 ReadUInt64();

        public Int16 ReadInt16();
        public Int32 ReadInt32();
        public Int64 ReadInt64();

        public float ReadSingle();
        public double ReadDouble();
        public decimal ReadDecimal();


        public Vector2 ReadVector2();
        public Vector3 ReadVector3();
        public Vector4 ReadVector4();
        public Matrix4x4 ReadMatrix4x4();
        public Quaternion ReadQuaternion();*/
    }
}