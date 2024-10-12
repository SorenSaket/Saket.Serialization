using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Saket.Serialization;

public class SerializationHasher : ISerializer
{
    public uint code;
    private const uint FNVPrime = 16777619u;
    private const uint FNVOffsetBasis = 2166136261u;


    bool ISerializer.IsReader => false;

    public long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public SerializationHasher()
    {
        code = 0;
    }

    public void Reset()
    {
        code = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private int SizeOf<T>()
    {
        if (typeof(T) == typeof(bool))
            return 1;
        if (typeof(T) == typeof(char))
            return 2;
        Type outputType = typeof(T).IsEnum ? Enum.GetUnderlyingType(typeof(T)) : typeof(T);
        return Marshal.SizeOf(outputType);
    }
    public static uint CombineHash(uint hash, uint value)
    {
        return hash ^ (value + 0x9e3779b9 + (hash << 6) + (hash >> 2));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Write(void* value, int length)
    {
        uint hash = FNVOffsetBasis;
        for (int i = 0; i < length; i++)
        {
            hash ^= ((byte*)value)[i];
            hash *= FNVPrime;
        }
        code = CombineHash(code, hash);
    }
    public void Serialize(ref Span<byte> value, ISerializer.ForBytes unused = default)
    {
        unsafe
        {
            var length = value.Length;
            Serialize(ref length);
            var a = (byte*)Unsafe.AsPointer(ref MemoryMarshal.GetReference(value));
            Write(a, value.Length);
        }
    }

    public void Serialize<T>(ref T value, ISerializer.ForUnmanaged unused = default) where T : unmanaged
    {
        unsafe
        {
            var a = (T*)Unsafe.AsPointer(ref value);
            Write(a, SizeOf<T>());
        }
    }

    public void Serialize<T>(ref Enum value, ISerializer.ForEnum unused = default) where T : Enum
    {
        unsafe
        {
            var a = (T*)Unsafe.AsPointer(ref value);
            Write(a, SizeOf<T>());
        }
    }

    public bool LoadBytes(int count)
    {
        return true;
    }
}

