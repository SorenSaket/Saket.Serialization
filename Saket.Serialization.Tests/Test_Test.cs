using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Serialization.Tests
{
    [TestClass]
    public  class Test_Test
    {
        [TestMethod]
        public void DoesThisShitWorkQuestionMark()
        {
            ulong val = (ulong)123123123123;
            FastWriter writer = new();

            writer.Write(val);

            FastReader reader = new(writer.DataRaw);



            Assert.AreEqual(val, reader.ReadUInt64());


        }
        [TestMethod]
        public void DoesThisShitWorkQuestionMarkElectricBoogaloo()
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            var reader = new BinaryReader(stream);

            sbyte expected = (sbyte)-103;

            writer.Write(expected);
            stream.Position = 0;

            sbyte actual = reader.ReadSByte();


            Assert.AreEqual(expected, actual);


            byte squeze = (byte)expected;

            sbyte result = (sbyte)squeze;

            Assert.AreEqual(expected, result);

        }
    }
}
