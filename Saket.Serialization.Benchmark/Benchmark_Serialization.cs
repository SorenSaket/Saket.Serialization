using BenchmarkDotNet.Attributes;
using Saket.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saket.Engine.Benchmark.Serialization
{
    [BenchmarkCategory("Reading")]
    public class Benchmark_Serialization_Read_UInt64
    {
        [Params(10000)]
        public int ItemCount { get; set; } = 10000;

        [Params(1)]
        public int InterationCount { get; set; } = 1;

        Stream stream;

        byte[] data;
        UInt64[] temp = new UInt64[16];

        [GlobalSetup]
        public void Setup()
        {
            data = new byte[ItemCount*8];
            Random rnd = new Random();
            rnd.NextBytes(data);
            stream = new MemoryStream(data);
        }
        
        /*
        [BenchmarkCategory("Saket")]
        [Benchmark]
        public void ASaket()
        {
            var reader = new Saket.Serialization.StreamReader(stream);

            reader.LoadBytes(ItemCount * 8);
            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                for (int y = 0; y < ItemCount; y++)
                {
                    temp[0] = reader.ReadUInt64();
                }
            }
        }*/
        
        /*
        [BenchmarkCategory("FastReader")]
        [Benchmark]
        public void FastReader()
        {
            var reader = new Saket.Serialization.FastReader(data);

            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                for (int y = 0; y < ItemCount; y++)
                {
                    temp[0] = reader.ReadUInt64();
                }
            }
        }*/

        [BenchmarkCategory("ByteReder")]
        [Benchmark(Baseline = true)]
        public void ByteReader()
        {
            var reader = new Saket.Serialization.ByteReader(data);

            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                for (int y = 0; y < ItemCount; y++)
                {
                    temp[0] = reader.ReadUInt64();
                }
            }
        }

        [BenchmarkCategory("ByteRederGeneric")]
        [Benchmark]
        public void ByteReaderGeneric()
        {
            var reader = new Saket.Serialization.ByteReader(data);

            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                for (int y = 0; y < ItemCount; y++)
                {
                    temp[0] = reader.Read<UInt64>();
                }
            }
        }


        /*
        [BenchmarkCategory("Saket Int")]
        [Benchmark]
        public void ASaketInterface()
        {
            var sr = new Saket.Serialization.StreamReader(stream);

            IReader r = sr;

            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                sr.LoadBytes(ItemCount * 8);
                for (int y = 0; y < ItemCount; y++)
                {
                    temp[0] = r.ReadUInt64();
                }
            }
        }

        [BenchmarkCategory("BinaryReader")]
        [Benchmark(Baseline = true)]
        public void BinaryReader()
        {
            var reader = new BinaryReader(stream);
           
            for (int i = 0; i < InterationCount; i++)
            {
                stream.Position = 0;
                for (int y = 0; y < ItemCount; y++)
                {
                    temp[0] = reader.ReadUInt64();
                }
            }
        }

      */
    }
}
