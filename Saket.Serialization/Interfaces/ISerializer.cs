using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization
{
    /// <summary>
    /// LoadBytes must be called before any serialize functions.
    /// </summary>
    public interface ISerializer
    {
        public long Position { get; set; }
        public bool IsReader { get; }
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
        public unsafe void SerializeBytes(ref byte* value, int count);


        public void SerializeBoolean(ref bool value);

        public void SerializeByte(ref byte value);
        public void SerializeSByte(ref sbyte value);

        public void SerializeChar(ref char value);

        /// <summary>
        /// The maximum length of the string is dependent on the implementation
        /// </summary>
        /// <param name="value"></param>
        public void SerializeString(ref string value);

        public void SerializeUInt16(ref UInt16 value);
        public void SerializeUInt32(ref UInt32 value);
        public void SerializeUInt64(ref UInt64 value);

        public void SerializeInt16(ref Int16 value);
        public void SerializeInt32(ref Int32 value);
        public void SerializeInt64(ref Int64 value);

        public void SerializeSingle(ref Single value);
        public void SerializeDouble(ref Double value);
        public void SerializeDecimal(ref Decimal value);

        public void SerializeVector2(ref Vector2 value);
        public void SerializeVector3(ref Vector3 value);
        public void SerializeVector4(ref Vector4 value);
        public void SerializeMatrix4x4(ref Matrix4x4 value);
        public void SerializeQuaternion(ref Quaternion value);

        //---- Generic ----
        public void SerializeUnmanaged<T>(ref T value) where T : unmanaged;
        public void SerializeEnum<T>(ref T value) where T : unmanaged, Enum;
        public void SerializeSerializable<T>(ref T value) where T : ISerializable, new();
    }
}