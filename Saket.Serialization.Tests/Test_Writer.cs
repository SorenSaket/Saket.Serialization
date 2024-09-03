using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Saket.Serialization.Tests;
#if false
[TestClass]
public class Test_Writer
{
    [TestMethod]
    public void Creation_Array()
    {
        var writer = new ByteSerializerWriter(77);

        Assert.AreEqual(0, writer.AbsolutePosition);
        Assert.AreEqual(77, writer.Capacity);
    }

    [TestMethod]
    public void Write_Primitive()
    {
        var writer = new ByteSerializerWriter();
        writer.Write(1.2f);
        Assert.AreEqual(4, writer.AbsolutePosition);
    }
    [TestMethod]
    public void Write_PrimitiveArray()
    {
        var writer = new ByteSerializerWriter();
        float[] values = new float[] { 1.2f, 23.4f, 22.0f };
        writer.Write(values);
        // length is 4*4
        Assert.AreEqual((4*4), writer.AbsolutePosition);
    }

    [TestMethod]
    public void Write_Enum()
    {
        var writer = new ByteSerializerWriter();


        var a = TestEnumUShort.max;
        writer.Serialize(ref a);
        Assert.AreEqual(2, writer.AbsolutePosition);
    }
    [TestMethod]
    public void Write_Serializable()
    {

    }

    [TestMethod]
    public void Expantion()
    {
        var writer = new ByteSerializerWriter(2);
        var a = 124.2f;
        writer.Serialize(ref a);
        Assert.IsTrue(writer.Capacity >= 4);
    }

}
#endif