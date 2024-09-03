using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization.Tests.Stream;
#if false
[TestClass]
public class Test_StreamWriter
{
    [TestMethod]
    public void Write()
    {
        MemoryStream stream = new MemoryStream();
        StreamWriterLE writer= new StreamWriterLE(stream);

        short a = -27;
        writer.SerializeInt16(ref a);

        stream.Position = 0;
        var reader = new BinaryReader(stream);

        Assert.AreEqual(a, reader.ReadInt16());
    }

}
#endif