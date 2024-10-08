﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization.Tests.Stream;
#if false
[TestClass]
public class Test_StreamSerialization
{
    [TestMethod]
    public void SerializableObject()
    {
        MemoryStream stream = new MemoryStream();

        StreamWriterLE writer= new StreamWriterLE(stream);

        var obj = new SerializableObject(0);

        writer.SerializeSerializable(ref obj);

        stream.Position = 0;
        var reader = new StreamReaderLE(stream);

        SerializableObject outva = new();
        reader.SerializeSerializable(ref outva);

        Assert.IsTrue(obj.Equals(outva));
    }
}
#endif