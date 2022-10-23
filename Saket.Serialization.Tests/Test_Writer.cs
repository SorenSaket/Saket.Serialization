using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Saket.Serialization.Tests
{
    [TestClass]
    public class Test_Writer
    {
        [TestMethod]
        public void Creation_Array()
        {
            var writer = new SerializerWriter(77);

            Assert.AreEqual(0, writer.AbsolutePosition);
            Assert.AreEqual(77, writer.Capacity);
        }

        [TestMethod]
        public void Write_Primitive()
        {
            var writer = new SerializerWriter();
            writer.Write(1.2f);
            Assert.AreEqual(4, writer.AbsolutePosition);
        }
        [TestMethod]
        public void Write_PrimitiveArray()
        {
            var writer = new SerializerWriter();
            float[] values = new float[] { 1.2f, 23.4f, 22.0f };
            writer.Write(values);
            // length is 4*4
            Assert.AreEqual((4*4), writer.AbsolutePosition);
        }

        [TestMethod]
        public void Write_Enum()
        {
            var writer = new SerializerWriter();
            writer.Write(TestEnumUShort.max);
            Assert.AreEqual(2, writer.AbsolutePosition);
        }
        [TestMethod]
        public void Write_Serializable()
        {

        }

        [TestMethod]
        public void Expantion()
        {
            var writer = new SerializerWriter(2);
            writer.Write(124.2f);

            Assert.IsTrue(writer.Capacity >= 4);
        }

    }
}
