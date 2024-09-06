using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Saket.Serialization;

/// <summary>
/// Writer struct able to serialize primities and ISerializables to byte array
/// </summary>
public struct ByteWriter : ISerializer
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
    /// <summary> The data that has been written to the underlying array </summary>
    public Span<byte> DataAsSpan
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new Span<byte>(data, 0, count);
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

    readonly bool ISerializer.IsReader => false;
    long ISerializer.Position { get => AbsolutePosition; set => AbsolutePosition = (int)value; }


    public ByteWriter(byte[] data)
    {
        this.data = data;
        count = 0;
        
    }
    public ByteWriter(int intialCapacity = 64)
    {
        this.data = new byte[intialCapacity];
        count = 0;
    }

    public void Reset()
    {
        absolutePosition = 0;
        count = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe void Write(void* value, int length)
    {
        Marshal.Copy(new IntPtr(value), data, absolutePosition, length);
        absolutePosition += length;
        // 
        count = Math.Max(count, absolutePosition);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal int SizeOf<T>()
    {
        if (typeof(T) == typeof(bool))
            return 1;
        if (typeof(T) == typeof(char))
            return 2;
        Type outputType = typeof(T).IsEnum ? Enum.GetUnderlyingType(typeof(T)) : typeof(T);
        return Marshal.SizeOf(outputType);
    }

    public bool LoadBytes(int count)
    {
        if (data.Length < absolutePosition + count)
            return false;
        return true;
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

}