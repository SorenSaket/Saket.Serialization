using System.Numerics;

namespace Saket.Serialization.Tests;

public enum TestEnum : UInt16
{
    Default = 0,
    SomewhereTheMiddle = 200,
    Max = UInt16.MaxValue
}

public struct SubObject : ISerializable, IEquatable<SubObject>
{
    public float value;

    public SubObject(float value)
    {
        this.value = value;
    }

    public override bool Equals(object? obj)
    {
        return obj is SubObject @object && Equals(@object);
    }

    public bool Equals(SubObject other)
    {
        return value == other.value;
    }

    public void Serialize(ISerializer serializer)
    {
        serializer.LoadBytes(4);
        serializer.Serialize(ref value);
    }

    public static bool operator ==(SubObject left, SubObject right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(SubObject left, SubObject right)
    {
        return !(left == right);
    }
}

public class SerializableObject : ISerializable, IEquatable<SerializableObject>
{
   
    public byte[] v1;

    public bool v2;

    public byte v3;
    public sbyte v4;

    public char v5;

    public string v6;

    public ushort v7;
    public uint v8;
    public ulong v9;

    public short v10;
    public int v11;
    public long v12;

    public float v13;
    public double v14;
    //public decimal v15;

    public Vector2 v16;
    public Vector3 v17;
    public Vector4 v18;
    public Matrix4x4 v19;
    public Quaternion v20;

    public TestEnum v21;
    public SubObject v22;
    public SerializableObject()
    {
        v1 = Array.Empty<byte>();
        v6 = string.Empty;
        v22 = default;
    }
    public SerializableObject(int seed)
    {
        Random rnd = new Random(seed);
        v1 = new byte[8];
        rnd.NextBytes(v1);

        v2 = rnd.Next(0, 2) == 0 ? false : true;
        v3 = (byte)rnd.Next();
        v4 = (sbyte)rnd.Next(sbyte.MinValue, sbyte.MaxValue);
        v5 = (char)rnd.Next(char.MinValue, char.MaxValue); 
        v6 = rnd.Next().ToString();

        v7 = (ushort)rnd.Next(ushort.MinValue, ushort.MaxValue);
        v8 = (uint)rnd.NextInt64(uint.MinValue, uint.MaxValue);
        v9 = (ulong)rnd.NextInt64();

        v10 = (short)rnd.Next(short.MinValue, short.MaxValue);
        v11 = (int)rnd.NextInt64(int.MinValue, int.MaxValue);
        v12 = (long)rnd.NextInt64();

        v13 = (float)rnd.NextDouble();
        v14 = (double)rnd.NextDouble();

        v16 = new Vector2((float)rnd.NextDouble(), (float)rnd.NextDouble());
        v17 = new Vector3((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
        v18 = new Vector4((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());

        v19 = new Matrix4x4(
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble(),
            (float)rnd.NextDouble()
            );

        v20 = new Quaternion((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());


        var ev = Enum.GetValues(typeof(TestEnum));
        v21 = (TestEnum) ev.GetValue(rnd.Next( 0, ev.Length));

        v22 = new SubObject((float)rnd.NextDouble());
    }

    public bool Equals(SerializableObject? other)
    {
        return
            v1.SequenceEqual(other.v1) &&
            v2 == other.v2 &&
            v3 == other.v3 &&
            v4 == other.v4 &&
            v5 == other.v5 &&
            v6 == other.v6 &&
            v7 == other.v7 &&
            v8 == other.v8 &&
            v9 == other.v9 &&
            v10 == other.v10 &&
            v11 == other.v11 &&
            v12 == other.v12 &&
            v13 == other.v13 &&
            v14 == other.v14 &&
            v16 == other.v16 &&
            v17 == other.v17 &&
            v18 == other.v18 &&
            v19 == other.v19 &&
            v20 == other.v20 &&
            v21 == other.v21 &&
            v22 == other.v22;
    }

    public void Serialize(ISerializer serializer)
    {
        serializer.Serialize(ref v1);

        serializer.LoadBytes(5);

        serializer.Serialize(ref v2);

        serializer.Serialize(ref v3);
        serializer.Serialize(ref v4);

        serializer.Serialize(ref v5);

        serializer.Serialize(ref v6);

        serializer.LoadBytes((2+4+8)*2 + 4 + 8 + (4*2) + (4 * 3) + (4 * 4) + (4 * 4 * 4) + (4 * 4)+2);

        serializer.Serialize(ref v7);
        serializer.Serialize(ref v8);
        serializer.Serialize(ref v9);

        serializer.Serialize(ref v10);
        serializer.Serialize(ref v11);
        serializer.Serialize(ref v12);

        serializer.Serialize(ref v13);
        serializer.Serialize(ref v14);

        serializer.Serialize(ref v16);
        serializer.Serialize(ref v17);
        serializer.Serialize(ref v18);
        serializer.Serialize(ref v19);
        serializer.Serialize(ref v20);

        serializer.Serialize(ref v21);

        serializer.Serialize(ref v22);
    }

}
