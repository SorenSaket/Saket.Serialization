using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Buffers;


namespace Saket.Serialization.Tests;
#if true

[TestClass]
public class Test_ReadWrite
{
    [TestMethod]
    public void ReadWrite_Primitive()
    {
        var writer = (ISerializer)new ByteWriter(512);
        var reader = (ISerializer)new ByteReader(((ByteWriter)writer).DataRaw);

        float data = 23125.123f;
        writer.Serialize(ref data);

        float data2 = 0;
        reader.Serialize(ref data2);

        Assert.AreEqual(data, data2);
    }
    [TestMethod]
    public void ReadWrite_PrimitiveArray()
    {
        var writer = (ISerializer)new ByteWriter(512);
        var reader = (ISerializer)new ByteReader(((ByteWriter)writer).DataRaw);

        float[] data = [2143.4f,7547.4f, 34653.1f];

        writer.Serialize(ref data);

        float[] data2 = [];

        reader.Serialize(ref data2);

        Assert.IsTrue(Enumerable.SequenceEqual(data, data2));
    }

    [TestMethod]
    public void ReadWrite_List()
    {
        var writer = (ISerializer)new ByteWriter(512);
        var reader = (ISerializer)new ByteReader(((ByteWriter)writer).DataRaw);

        List<float> data = [2143.4f, 7547.4f, 34653.1f];

        writer.Serialize(ref data);

        List<float> data2 = [43];

        reader.Serialize(ref data2);

        Assert.IsTrue(Enumerable.SequenceEqual(data, data2));
    }

    [TestMethod]
    public void ReadWrite_PrimitiveDictionary()
    {
        var writer = (ISerializer)new ByteWriter(512);
        var reader = (ISerializer)new ByteReader(((ByteWriter)writer).DataRaw);

        Dictionary<uint, float> data = new()
        {
            { 4, 4.1230f },
            { 1254, 856f }
        };

        writer.Serialize(ref data);

        Dictionary<uint, float> data2 = new(){
            { 1254, 9f }
        };

        reader.Serialize(ref data2);

        Assert.IsTrue(Enumerable.SequenceEqual(data, data2));
    }
    [TestMethod]
    public void ReadWrite_Enum()
    {
        var writer = (ISerializer)new ByteWriter(512);
        var reader = (ISerializer)new ByteReader(((ByteWriter)writer).DataRaw);

        TestEnum data = TestEnum.Max;

        writer.Serialize(ref data);

        TestEnum data2 = TestEnum.Default;

        reader.Serialize(ref data2);

        Assert.AreEqual(data, data2);
    }
    [TestMethod]
    public void ReadWrite_String()
    {
        var writer = (ISerializer)new ByteWriter(512);
        var reader = (ISerializer)new ByteReader(((ByteWriter)writer).DataRaw);

        String data = "testdata";

        writer.Serialize(ref data);

        string data2 = "";

        reader.Serialize(ref data2);

        Assert.AreEqual(data, data2);
    }
    [TestMethod]
    public void ReadWrite_Serializable()
    {
        var writer = (ISerializer)new ByteWriter(512);
        var reader = (ISerializer)new ByteReader(((ByteWriter)writer).DataRaw);

        SerializableObject data = new SerializableObject(123);
     
        writer.Serialize(ref data);

        SerializableObject data2 = new SerializableObject();
        
        reader.Serialize(ref data2);

        Assert.IsTrue(data.Equals(data2));
    }
}
#endif