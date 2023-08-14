using System;
using System.Numerics;

namespace Saket.Serialization
{
    /// <summary>
    /// LoadBytes must be called before any serialize functions.
    /// </summary>
    public interface ISerializer
    {
        public long Position { get; set; }
        /// <summary>
        /// Whether is serializer is a reader.
        /// </summary>
        public bool IsReader { get; }
        /// <summary>
        /// Whether is serializer is a writer. 
        /// </summary>
        public bool IsWriter => !IsReader;

        /// <summary>
        /// Ensure that the number of bytes is serializable. 
        /// This method must be called before any serialize functions to ensure bounds.
        /// </summary>
        /// <param name="count"></param>
        public bool LoadBytes(int count);

        /// <summary>
        /// Serialize arbitrary number of bytes 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public void SerializeBytes(ref byte[] value, int offset = 0, int count = 0);
        /// <summary>
        /// Serialize arbitrary number of bytes from ptr
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public unsafe void SerializeBytes(byte* value, int count);

        /*
        public void SerializeBoolean(ref bool value);

        public void SerializeByte(ref byte value);
        public void SerializeSByte(ref sbyte value);

        public void SerializeChar(ref char value);

        /// <summary>
        /// The maximum length of the string is dependent on the implementation
        /// </summary>
        /// <param name="value"></param>
        public void SerializeString(ref string value);

        public void SerializeUInt16(ref ushort value);
        public void SerializeUInt32(ref uint value);
        public void SerializeUInt64(ref ulong value);

        public void SerializeInt16(ref short value);
        public void SerializeInt32(ref int value);
        public void SerializeInt64(ref long value);

        public void SerializeSingle(ref float value);
        public void SerializeDouble(ref double value);
        public void SerializeDecimal(ref decimal value);

        public void SerializeVector2(ref Vector2 value);
        public void SerializeVector3(ref Vector3 value);
        public void SerializeVector4(ref Vector4 value);
        public void SerializeMatrix4x4(ref Matrix4x4 value);
        public void SerializeQuaternion(ref Quaternion value);*/

        //---- Generic ----
        public void Serialize<T>(ref T value) where T : unmanaged;
        public void Serialize<T>(ref T[] value) where T : unmanaged
        {
            unsafe
            {
                int length = 0;
                Serialize(ref length);
                if (value.Length < length)
                    value = new T[length];

                for (int i = 0; i < length; i++)
                {
                    Serialize<T>(ref value[i]);
                }
            }
        }

        public void SerializeEnum<T>(ref T value) where T : unmanaged, Enum;

        public void SerializeSerializable<T>(ref T value) where T : ISerializable, new()
        { 
            value.Serialize(this); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        public void SerializeSerializable<T>(ref T[] value) where T : ISerializable, new()
        {
            unsafe
            {
                int length = 0;
                Serialize(ref length);

                if (value.Length < length)
                {
                    value = new T[length];
                }

                for (int i = 0; i < length; i++)
                {
                    SerializeSerializable<T>(ref value[i]);
                }
            }
        }
    }
}