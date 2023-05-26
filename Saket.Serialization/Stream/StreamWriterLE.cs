using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;

namespace Saket.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    public class StreamWriterLE : BaseStreamWriter,  ISerializer
    {
        public StreamWriterLE(Stream input) : base(input)
        {
        }

        #region Serialization Functions


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeBytes(ref byte[] value, int offset = 0, int count = 0)
        {
            if (count < 0)
                return;
            if (value == null)
                return;
            SerializeInt32(ref count);
            Stream.Write(value, offset, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe void SerializeBytes(ref byte* value, int count)
        {
            throw new NotImplementedException();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeBoolean(ref bool value)
        {
           Stream.WriteByte(value ? (byte)1 : (byte)0);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeByte(ref byte value)
        {
            Stream.WriteByte(value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeSByte(ref sbyte value)
        {
            Stream.WriteByte((byte)value);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeChar(ref char value)
        {
            Buffer[0] = (byte)(value);
            Buffer[1] = (byte)(value >> 8);
            Stream.Write(Buffer, 0, 2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeString(ref string value)
        {
            int length = value.Length;
            // Write the length of the string
            SerializeInt32(ref length);
            
            for (int i = 0; i < length; i++)
            {
                char c = value[i];
                SerializeChar(ref c);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeInt16(ref short value)
        {
            Buffer[0] = (byte)(value >> 0);
            Buffer[1] = (byte)(value >> 8);
            Stream.Write(Buffer, 0, 2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeInt32(ref int value)
        {
            Buffer[0] = (byte)(value >> 0);
            Buffer[1] = (byte)(value >> 8);
            Buffer[2] = (byte)(value >> 16);
            Buffer[3] = (byte)(value >> 24);
            Stream.Write(Buffer, 0, 4);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeInt64(ref long value)
        {
            Buffer[0] = (byte)value;
            Buffer[1] = (byte)(value >> 8);
            Buffer[2] = (byte)(value >> 16);
            Buffer[3] = (byte)(value >> 24);
            Buffer[4] = (byte)(value >> 32);
            Buffer[5] = (byte)(value >> 40);
            Buffer[6] = (byte)(value >> 48);
            Buffer[7] = (byte)(value >> 56);
            Stream.Write(Buffer, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUInt16(ref ushort value)
        {
            Buffer[0] = (byte)(value >> 0);
            Buffer[1] = (byte)(value >> 8);
            Stream.Write(Buffer, 0, 2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUInt32(ref uint value)
        {
            Buffer[0] = (byte)(value >> 0);
            Buffer[1] = (byte)(value >> 8);
            Buffer[2] = (byte)(value >> 16);
            Buffer[3] = (byte)(value >> 24);
            Stream.Write(Buffer, 0, 4);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUInt64(ref ulong value)
        {
            Buffer[0] = (byte)value;
            Buffer[1] = (byte)(value >> 8);
            Buffer[2] = (byte)(value >> 16);
            Buffer[3] = (byte)(value >> 24);
            Buffer[4] = (byte)(value >> 32);
            Buffer[5] = (byte)(value >> 40);
            Buffer[6] = (byte)(value >> 48);
            Buffer[7] = (byte)(value >> 56);
            Stream.Write(Buffer, 0, 8);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeSingle(ref float value)
        {
           unsafe {
                fixed(float* ptr = &value)
                {
                    byte* TmpValue = (byte*)ptr;
                    Buffer[0] = TmpValue[0];
                    Buffer[1] = TmpValue[1];
                    Buffer[2] = TmpValue[2];
                    Buffer[3] = TmpValue[3];
                    Stream.Write(Buffer, 0, 4);
                }
           }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeDouble(ref double value)
        {
            unsafe
            {
                fixed (double* ptr = &value)
                {
                    byte* TmpValue = (byte*)ptr;
                    Buffer[0] = TmpValue[0];
                    Buffer[1] = TmpValue[1];
                    Buffer[2] = TmpValue[2];
                    Buffer[3] = TmpValue[3];
                    Buffer[4] = TmpValue[4];
                    Buffer[5] = TmpValue[5];
                    Buffer[6] = TmpValue[6];
                    Buffer[7] = TmpValue[7];
                    Stream.Write(Buffer, 0, 8);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeDecimal(ref decimal value)
        {
            // Go thank Microsoft for making such a shitty decial api
            throw new NotImplementedException();
        }
 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeVector2(ref Vector2 value)
        {
            unsafe
            {
                fixed (Vector2* ptr = &value)
                {
                    byte* TmpValue = (byte*)ptr;
                    Buffer[0] = TmpValue[0];
                    Buffer[1] = TmpValue[1];
                    Buffer[2] = TmpValue[2];
                    Buffer[3] = TmpValue[3];
                    Buffer[4] = TmpValue[4];
                    Buffer[5] = TmpValue[5];
                    Buffer[6] = TmpValue[6];
                    Buffer[7] = TmpValue[7];
                    Stream.Write(Buffer, 0, 8);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeVector3(ref Vector3 value)
        {
            unsafe
            {
                fixed (Vector3* ptr = &value)
                {
                    byte* TmpValue = (byte*)ptr;
                    Buffer[0] = TmpValue[0];
                    Buffer[1] = TmpValue[1];
                    Buffer[2] = TmpValue[2];
                    Buffer[3] = TmpValue[3];
                    Buffer[4] = TmpValue[4];
                    Buffer[5] = TmpValue[5];
                    Buffer[6] = TmpValue[6];
                    Buffer[7] = TmpValue[7];
                    Buffer[8] = TmpValue[8];
                    Buffer[9] = TmpValue[9];
                    Buffer[10] = TmpValue[10];
                    Buffer[11] = TmpValue[11];
                    Stream.Write(Buffer, 0, 12);
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeVector4(ref Vector4 value)
        {
            unsafe
            {
                fixed (Vector4* ptr = &value)
                {
                    fixed (byte* b = Buffer)
                    {
                        if (AdvSimd.IsSupported)
                        {
                            AdvSimd.Store(b, AdvSimd.LoadVector128((byte*)ptr));
                        }
                        else if (Sse2.IsSupported)
                        {
                            Sse2.Store(b, Sse2.LoadVector128((byte*)ptr));
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        Stream.Write(Buffer, 0, 16);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeMatrix4x4(ref Matrix4x4 value)
        {
            unsafe
            {
                fixed (Matrix4x4* ptr = &value)
                {
                    fixed (byte* b = Buffer)
                    {
                        if (AdvSimd.IsSupported)
                        {
                            AdvSimd.Store(&b[0], AdvSimd.LoadVector128(&((byte*)ptr)[0]));
                            AdvSimd.Store(&b[16], AdvSimd.LoadVector128(&((byte*)ptr)[16]));
                            AdvSimd.Store(&b[32], AdvSimd.LoadVector128(&((byte*)ptr)[32]));
                            AdvSimd.Store(&b[48], AdvSimd.LoadVector128(&((byte*)ptr)[48]));
                        }
                        else if (Avx.IsSupported)
                        {
                            Avx.Store(&b[0], Avx.LoadVector256( &((byte*)ptr)[0]) );
                            Avx.Store(&b[32], Avx.LoadVector256(&((byte*)ptr)[32]) );
                        }
                        else if (Sse2.IsSupported)
                        {
                            Sse2.Store(&b[0],   Sse2.LoadVector128(&((byte*)ptr)[0]));
                            Sse2.Store(&b[16],  Sse2.LoadVector128(&((byte*)ptr)[16]));
                            Sse2.Store(&b[32],  Sse2.LoadVector128(&((byte*)ptr)[32]));
                            Sse2.Store(&b[48],  Sse2.LoadVector128(&((byte*)ptr)[48]));
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        Stream.Write(Buffer, 0, 64);
                    }
                }
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeQuaternion(ref Quaternion value)
        {
            unsafe
            {
                fixed (Quaternion* ptr = &value)
                {
                    fixed (byte* b = Buffer)
                    {
                        if (AdvSimd.IsSupported)
                        {
                            AdvSimd.Store(b, AdvSimd.LoadVector128((byte*)ptr));
                        }
                        else if (Sse2.IsSupported)
                        {
                            Sse2.Store(b, Sse2.LoadVector128((byte*)ptr));
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                        Stream.Write(Buffer, 0, 16);
                    }
                }
            }
        }

        #endregion

        #region Generic Serialization Functions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUnmanaged<T>(ref T value) where T : unmanaged
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeSerializable<T>(ref T value) where T : ISerializable, new()
        {
            value.Serialize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeEnum<T>(ref T value) where T : unmanaged, Enum
        {
            Type t = Enum.GetUnderlyingType(typeof(T));
            int length = Marshal.SizeOf(t);

            unsafe
            {
                fixed(T* ptr = &value)
                {
                    byte* bytes = (byte*)ptr;
                    for (int i = 0; i < length; i++)
                    {
                        Buffer[i] = (byte)bytes[i];
                    }
                }
            }

            Stream.Write(Buffer, 0, length);
        }


        #endregion
    }
}