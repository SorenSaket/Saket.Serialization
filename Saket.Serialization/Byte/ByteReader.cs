using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Saket.Serialization;

/// <summary>
/// Read directly from a byte Array.
/// </summary>
public struct ByteReader : ISerializer
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


    long ISerializer.Position { get => RelativePosition; set => RelativePosition = (int)value; }
    readonly bool ISerializer.IsReader => true;


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
        if (typeof(T) == typeof(bool))
            return 1;
        if (typeof(T) == typeof(char))
            return 2;
        Type outputType = typeof(T).IsEnum ? Enum.GetUnderlyingType(typeof(T)) : typeof(T);
        return Marshal.SizeOf(outputType);
    }


    public void Serialize(ref Span<byte> value, ISerializer.ForBytes unused = default)
    {
        // read length
        int length = 0;
        Serialize(ref length);
        value = new Span<byte>(data, absolutePosition, length);
        Advance(length);
    }

    public void Serialize<T>(ref T value, ISerializer.ForUnmanaged unused = default) where T : unmanaged
    {
        unsafe
        {
            fixed (byte* p = data)
            {
                value = Unsafe.Read<T>(p + absolutePosition);
                Advance(SizeOf<T>());
            }
        }
    }

    public void Serialize<T>(ref Enum value, ISerializer.ForEnum unused = default) where T : Enum
    {
        int position = absolutePosition;
        Type t = Enum.GetUnderlyingType(typeof(T));
        unsafe
        {
            fixed (byte* p = data)
            {
                value = (T)Marshal.PtrToStructure(new IntPtr(p + position), t)!;
                Advance(SizeOf<T>());
            }
        }
    }

    public bool LoadBytes(int count)
    {
        return true;
    }
}
