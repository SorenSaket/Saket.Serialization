using System;
using System.Collections.Generic;

namespace Saket.Serialization;

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


    //---- Bytes ----
    public void Serialize(ref Span<byte> value, ForBytes unused = default);
    public unsafe void Serialize(byte* value, int count)
    {
        var span = new Span<byte>(value, count);
        Serialize(ref span);
    }

    // ---- String ----
    public void Serialize(ref string value)
    {
        int length = value.Length;
        Serialize(ref length);

        Span<char> chars = stackalloc char[length];
        if(IsWriter)
            value.AsSpan().CopyTo(chars);

        for (int i = 0; i < length; i++)
        {
            Serialize(ref chars[i]);
        }

        value = new string(chars);
    }

    // ---- Generic Unmanged ----
    public void Serialize<T>(ref T value, ForUnmanaged unused = default) where T : unmanaged;
    public void Serialize<T>(ref T[] value, ForUnmanaged unused = default) where T : unmanaged
    {
        int length = value.Length;
        Serialize(ref length);
        if (value.Length < length)
            value = new T[length];

        for (int i = 0; i < length; i++)
        {
            Serialize(ref value[i]);
        }
    }
    public void Serialize<T>(ref List<T> list, ForUnmanaged unused = default) where T : unmanaged
    {
        int length = list.Count;
        Serialize(ref length);
        
        list.Capacity = length;

        // Length now contains the length of the dictionary
        for (int i = 0; i < length; i++)
        {
            T value = default;
            Serialize(ref value);
            list[i] = value;
        }
    }
    public void Serialize<K, V>(ref Dictionary<K, V> dictionary, ForUnmanaged unused = default)
         where K : unmanaged
         where V : unmanaged
    {
        int length = dictionary.Count;
        Serialize(ref length);
        // Length now contains the length of the dictionary
        if (IsReader)
        {
            // Clear the old one
            dictionary.Clear();
            for (int i = 0; i < length; i++)
            {
                K key = default;
                Serialize(ref key);

                V value = default;
                Serialize(ref value);

                 dictionary.Add(key, value);
            }
        }
        else
        {
            foreach (var item in dictionary)
            {
                K key = item.Key;
                Serialize(ref key);

                V value = item.Value;
                Serialize(ref value);
            }
        }
    }

    // ---- Enum ----
    public void Serialize<T>(ref Enum value, ForEnum unused = default) where T : Enum;

    // ---- Serializable ----
    public void Serialize<T>(ref T value) where T : ISerializable, new()
    { 
        value.Serialize(this); 
    }
    public void Serialize<T>(ref T[] value) where T : ISerializable, new()
    {
        int length = value.Length;
        Serialize(ref length);

        if (value.Length < length)
        {
            value = new T[length];
        }

        for (int i = 0; i < length; i++)
        {
            Serialize(ref value[i]);
        }
    }
    public void Serialize<T>(ref List<T> list) where T : ISerializable, new()
    {
        int length = list.Count;
        Serialize(ref length);

        list.Capacity = length;

        // Length now contains the length of the dictionary
        for (int i = 0; i < length; i++)
        {
            T value = new();
            Serialize(ref value);
            list[i] = value;
        }
    }
    public void Serialize<K, V>(ref Dictionary<K, V> dictionary)
        where K : unmanaged
        where V : ISerializable, new()
    {
        int length = dictionary.Count;
        Serialize(ref length);
        // Length now contains the length of the dictionary
        if (IsReader)
        {
            // Clear the old one
            dictionary.Clear();
            for (int i = 0; i < length; i++)
            {
                K key = default;
                Serialize(ref key);

                V value = new();
                Serialize(ref value);

                dictionary.Add(key, value);
            }
        }
        else
        {
            foreach (var item in dictionary)
            {
                K key = item.Key;
                Serialize(ref key);

                V value = item.Value;
                Serialize(ref value);
            }
        }
    }

    struct ForBytes { }
    struct ForUnmanaged { }
    struct ForEnum { }
    struct ForSerializable { }
}