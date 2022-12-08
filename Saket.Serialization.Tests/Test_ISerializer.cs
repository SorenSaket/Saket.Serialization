using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Buffers;


namespace Saket.Serialization.Tests
{
    [TestClass]
    public class Test_ISerializer
    {
        [TestMethod]
        public void StreamReader()
        {
            byte[] buffer = new byte[128];

            var stream = new MemoryStream();

            var a = new BinaryWriter(stream);

            a.Write(true);
            a.Write((byte)144);
            a.Write((sbyte)-3);
            a.Write('a');

            stream.Position = 0;
            var streamReader = new SStreamReader(stream);
            streamReader.LoadBytes(61);

            {
                bool val = default;
                streamReader.SerializeBoolean(ref val);
                Assert.AreEqual(true, val);
            }

            {
                byte val = default;
                streamReader.SerializeByte(ref val);
                Assert.AreEqual((byte)144, val);
            }

            {
                sbyte val = default;
                streamReader.SerializeSByte(ref val);
                Assert.AreEqual((sbyte)-3, val);
            }

            {
                char val = default;
                streamReader.SerializeChar(ref val);
                Assert.AreEqual('a', val);
            }

            /*
            a.Write((UInt16)4244);
            a.Write((UInt32)634673);
            a.Write((UInt64)96749469);

            a.Write((Int16)(-4244));
            a.Write((Int32)(-634673));
            a.Write((Int64)(-96749469));

            a.Write(2541.2f);
            a.Write((double)-23522541.2f);
            a.Write((decimal)2359259.2f);*/

        }


    }
}