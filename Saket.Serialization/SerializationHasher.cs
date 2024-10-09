using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization;

public class SerializationHasher : ISerializer
{
    public HashCode HashCode;



    bool ISerializer.IsReader => false;

    public long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public SerializationHasher()
    {
        HashCode = new HashCode();
    }

    public void Reset()
    {
        HashCode = new HashCode();
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


    public void Serialize(ref Span<byte> value, ISerializer.ForBytes unused = default)
    {
        HashCode.AddBytes(value);
    }

    public void Serialize<T>(ref T value, ISerializer.ForUnmanaged unused = default) where T : unmanaged
    {
        HashCode.Add(value);
    }

    public void Serialize<T>(ref Enum value, ISerializer.ForEnum unused = default) where T : Enum
    {
        HashCode.Add(value);
    }

    public bool LoadBytes(int count)
    {
        return true;
    }
}

