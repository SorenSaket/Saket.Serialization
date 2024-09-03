using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Saket.Serialization.Tests;

#if false
[TestClass]
public class Test_Reader
{
    [TestMethod]
    public void Creation_Array()
    {
        byte[] data = new byte[64];
        var reader = new ByteSerializerReader(data,10);

        Assert.AreEqual(10, reader.AbsolutePosition);
        Assert.AreEqual(0, reader.RelativePosition);

    }
    [TestMethod]
    public void Creation_ArraySegment()
    {
        byte[] data = new byte[64];
        var reader = new ByteSerializerReader(new ArraySegment<byte>(data, 10, 12));

        Assert.AreEqual(10, reader.AbsolutePosition);
        Assert.AreEqual(0, reader.RelativePosition);
        Assert.AreEqual(12, reader.Capacity);
    }

    [TestMethod]
    public void Read_Primitive()
    {
        byte[] data = new byte[64];
        var reader = new ByteSerializerReader( data,4);
        reader.Read<float>();
        Assert.AreEqual(8, reader.AbsolutePosition);
        Assert.AreEqual(4, reader.RelativePosition);
    }
    [TestMethod]
    public void Read_Enum()
    {
        byte[] data = new byte[64];
        var reader = new ByteSerializerReader( data, 4);

        reader.Read<TestEnumUShort>();

        Assert.AreEqual(6, reader.AbsolutePosition);
        Assert.AreEqual(2, reader.RelativePosition);
    }
    [TestMethod]
    public void Read_Serializable()
    {
        
    }

    [TestMethod]
    public void DebugSafety()
    {
        Assert.ThrowsException<IndexOutOfRangeException>(() => {
            byte[] data = new byte[64];
            var reader = new ByteReader( data, 64);
            reader.Read<float>(); 
        });
    }

}
#endif