using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization.Tests.Stream
{
    [TestClass]
    public class Test_StreamSerialization
    {
        [TestMethod]
        public void SerializableObject()
        {
            MemoryStream stream = new MemoryStream();

            SStreamWriter writer= new SStreamWriter(stream);

            var obj = new SerializableObject(0);

            writer.SerializeSerializable(ref obj);

            stream.Position = 0;
            var reader = new SStreamReader(stream);

            SerializableObject outva = new();
            reader.SerializeSerializable(ref outva);

            Assert.IsTrue(obj.Equals(outva));
        }
    }
}
