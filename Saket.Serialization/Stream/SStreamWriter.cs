using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization
{
    public class SStreamWriter : ISerializer
    {
        public long Position { get=> Stream.Position; set => Stream.Position = value; }
        public bool IsReader => false;
        public Stream Stream { get; protected set; }
        public byte[] Buffer { get => buffer; }
        
        protected byte[] buffer;


        public SStreamWriter(Stream input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            Stream = input;
            buffer = new byte[64];
        }

        /// <summary>
        /// Writer doesn't need to load bytes
        /// </summary>
        /// <param name="numberOfBytes"></param>
        /// <returns></returns>
        public virtual bool LoadBytes(int numberOfBytes)
        {
            return true;
        }
        
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
            buffer[0] = (byte)(value);
            buffer[1] = (byte)(value >> 8);
            Stream.Write(buffer, 0, 2);
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
            buffer[0] = (byte)(value >> 0);
            buffer[1] = (byte)(value >> 8);
            Stream.Write(buffer, 0, 2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeInt32(ref int value)
        {
            buffer[0] = (byte)(value >> 0);
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 16);
            buffer[3] = (byte)(value >> 24);
            Stream.Write(buffer, 0, 4);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeInt64(ref long value)
        {
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 16);
            buffer[3] = (byte)(value >> 24);
            buffer[4] = (byte)(value >> 32);
            buffer[5] = (byte)(value >> 40);
            buffer[6] = (byte)(value >> 48);
            buffer[7] = (byte)(value >> 56);
            Stream.Write(buffer, 0, 8);
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUInt16(ref ushort value)
        {
            buffer[0] = (byte)(value >> 0);
            buffer[1] = (byte)(value >> 8);
            Stream.Write(buffer, 0, 2);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUInt32(ref uint value)
        {
            buffer[0] = (byte)(value >> 0);
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 16);
            buffer[3] = (byte)(value >> 24);
            Stream.Write(buffer, 0, 4);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeUInt64(ref ulong value)
        {
            buffer[0] = (byte)value;
            buffer[1] = (byte)(value >> 8);
            buffer[2] = (byte)(value >> 16);
            buffer[3] = (byte)(value >> 24);
            buffer[4] = (byte)(value >> 32);
            buffer[5] = (byte)(value >> 40);
            buffer[6] = (byte)(value >> 48);
            buffer[7] = (byte)(value >> 56);
            Stream.Write(buffer, 0, 8);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void SerializeSingle(ref float value)
        {
           unsafe {
                fixed(float* ptr = &value)
                {
                    byte* TmpValue = (byte*)ptr;
                    buffer[0] = TmpValue[0];
                    buffer[1] = TmpValue[1];
                    buffer[2] = TmpValue[2];
                    buffer[3] = TmpValue[3];
                    Stream.Write(buffer, 0, 4);
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
                    buffer[0] = TmpValue[0];
                    buffer[1] = TmpValue[1];
                    buffer[2] = TmpValue[2];
                    buffer[3] = TmpValue[3];
                    buffer[4] = TmpValue[4];
                    buffer[5] = TmpValue[5];
                    buffer[6] = TmpValue[6];
                    buffer[7] = TmpValue[7];
                    Stream.Write(buffer, 0, 8);
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
                    buffer[0] = TmpValue[0];
                    buffer[1] = TmpValue[1];
                    buffer[2] = TmpValue[2];
                    buffer[3] = TmpValue[3];
                    buffer[4] = TmpValue[4];
                    buffer[5] = TmpValue[5];
                    buffer[6] = TmpValue[6];
                    buffer[7] = TmpValue[7];
                    Stream.Write(buffer, 0, 8);
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
                    buffer[0] = TmpValue[0];
                    buffer[1] = TmpValue[1];
                    buffer[2] = TmpValue[2];
                    buffer[3] = TmpValue[3];
                    buffer[4] = TmpValue[4];
                    buffer[5] = TmpValue[5];
                    buffer[6] = TmpValue[6];
                    buffer[7] = TmpValue[7];
                    buffer[8] = TmpValue[8];
                    buffer[9] = TmpValue[9];
                    buffer[10] = TmpValue[10];
                    buffer[11] = TmpValue[11];
                    Stream.Write(buffer, 0, 12);
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
                    fixed (byte* b = buffer)
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
                        Stream.Write(buffer, 0, 16);
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
                    fixed (byte* b = buffer)
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

                        Stream.Write(buffer, 0, 64);
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
                    fixed (byte* b = buffer)
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
                        Stream.Write(buffer, 0, 16);
                    }
                }
            }
        }

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
                        buffer[i] = (byte)bytes[i];
                    }
                }
            }

            Stream.Write(buffer, 0, length);
        }


        #endregion
    }
}