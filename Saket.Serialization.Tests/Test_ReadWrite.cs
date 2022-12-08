using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Buffers;


namespace Saket.Serialization.Tests
{
    [TestClass]
    public class Test_ReadWrite
    {
        [TestMethod]
        public void ReadWrite_Primitive()
        {
            var writer = new ByteWriter();
            var reader = new ByteReader(writer.DataRaw);
        
            float data = 23125.123f;
            writer.Write(data);

            Assert.AreEqual(data, reader.Read<float>());
        }
        [TestMethod]
        public void ReadWrite_PrimitiveArray()
        {
            var writer = new ByteWriter();
            var reader = new ByteReader(writer.DataRaw);

            float[] data = new float[] {2143.4f,7547.4f, 34653.1f};
            writer.Write(data);
           
            Assert.IsTrue(Enumerable.SequenceEqual(data, reader.ReadArray<float>()));
        }


        [TestMethod]
        public void ReadWrite_Enum()
        {
            var writer = new ByteWriter();
            var reader = new ByteReader(writer.DataRaw);

            TestEnumUShort data = TestEnumUShort.max;
            writer.Write(data);

            Assert.AreEqual(data, reader.Read<TestEnumUShort>());
        }

        [TestMethod]
        public void ReadWrite_Serializable()
        {
            var writer = new ByteWriter();
            var reader = new ByteReader(writer.DataRaw);

            TestSerializable data = new TestSerializable(253,6437, new int[] { 2143, 7547, 34653 });
            writer.WriteSerializable(data);

            var readData = reader.ReadSerializable<TestSerializable>();

            Assert.AreEqual(data, readData);
        }
        [TestMethod]
        public void ReadWrite_SerializableArray()
        {
            var writer = new ByteWriter();
           

            TestSerializable[] data = new TestSerializable[] {
                new(253, 6437, new int[] { 2143, 7547, 34653 }),
                new(455, 5733245, new int[] { 685, 678, 12312 }),
                new(679, 50875, new int[] { 12302, 9789, 678 }),
            };

            writer.WriteSerializable(data);

            var reader = new ByteReader(writer.DataRaw);
            var readData = reader.ReadSerializableArray<TestSerializable>();
            
            Assert.IsTrue(Enumerable.SequenceEqual(data, readData));

            ArrayBufferWriter<byte> rew = new ArrayBufferWriter<byte>();
            rew.Write(BitConverter.GetBytes(23.1f));
           
        }


    }
}