using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization
{
    /// <summary>
    /// A Reader can convert a series of bytes into primitive values.
    /// A Reader automatically advances position when reading values.
    /// </summary>
    public interface IReader
    {
        public int Position { get; set; }

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
        public Quaternion ReadQuaternion();



        /// <summary>
        /// Read abetrary number of bytes 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public ArraySegment<byte> Read(int length);

        /// <summary>
        /// Read Serializable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadSerializable<T>() where T : ISerializable;
    }
}